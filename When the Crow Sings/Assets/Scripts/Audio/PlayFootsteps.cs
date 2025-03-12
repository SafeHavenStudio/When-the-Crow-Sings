using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Rendering;
using FMOD.Studio;

public class PlayFootsteps : MonoBehaviour
{
    public PlayerController player;
    float maxDistance = 10f;

    [field: SerializeField] public EventReference ConcreteFootsteps { get; private set; }
    [field: SerializeField] public EventReference GrassFootsteps { get; private set; }
    [field: SerializeField] public EventReference IndoorFootsteps { get; private set; }
    [field: SerializeField] public EventReference DefaultFootsteps { get; private set; }
    [field: SerializeField] public EventReference DirtFootsteps { get; private set; }
    [field: SerializeField] public EventReference GravelFootsteps { get; private set; }
    [field: SerializeField] public EventReference LeafFootsteps { get; private set; }

    private EventReference SelectedFootsteps; // Stores the assigned footstep sound

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance))
        {
            Debug.DrawRay(transform.position, Vector3.down * maxDistance, Color.green);

            Terrain terrain = hit.collider.GetComponent<Terrain>();
            if (terrain != null)
            {
                //Handle terrain painted textures
                int textureIndex = GetMainTextureIndex(terrain, hit.point);
                AssignFootstepSoundFromTerrain(textureIndex, terrain.terrainData);
            }
            else
            {
                // Handle mesh-based textures
                Renderer groundRenderer = hit.collider.GetComponent<Renderer>();
                if (groundRenderer != null)
                {
                    Material material = groundRenderer.sharedMaterial;
                    if (material != null && material.mainTexture is Texture2D texture)
                    {
                        string textureName = texture.name.ToLower();
                        //Debug.Log("Detected Texture: " + textureName);
                        AssignFootstepSound(textureName);
                    }
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * maxDistance, Color.red);
            //Debug.Log("No texture detected, using default footsteps.");
            SelectedFootsteps = DefaultFootsteps;
        }
    }

    int GetMainTextureIndex(Terrain terrain, Vector3 worldPos)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        // Convert world position to terrain coordinates
        int mapX = Mathf.FloorToInt((worldPos.x - terrainPos.x) / terrainData.size.x * terrainData.alphamapWidth);
        int mapZ = Mathf.FloorToInt((worldPos.z - terrainPos.z) / terrainData.size.z * terrainData.alphamapHeight);

        float[,,] alphaMaps = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        int dominantTextureIndex = 0;
        float maxAlpha = 0f;

        // Find the texture with the highest influence
        for (int i = 0; i < terrainData.alphamapLayers; i++)
        {
            if (alphaMaps[0, 0, i] > maxAlpha)
            {
                maxAlpha = alphaMaps[0, 0, i];
                dominantTextureIndex = i;
            }
        }

        return dominantTextureIndex;
    }

    void AssignFootstepSoundFromTerrain(int textureIndex, TerrainData terrainData)
    {
        if (terrainData.terrainLayers.Length > textureIndex)
        {
            string textureName = terrainData.terrainLayers[textureIndex].diffuseTexture.name.ToLower();
            //Debug.Log("Terrain texture detected: " + textureName);

            switch (textureName)
            {
                case "tex_reccenterinterior_base":
                case "gh_interiorframe_basemap":
                case "ehq_interior_basemap":
                case "tex_powerctrlinterior":
                case "tex_scientisthouseint":
                    SelectedFootsteps = ConcreteFootsteps;
                    break;
                case "tex_dirt":
                case "tex_dirt_grey":
                case "tex_dirt_lightbrown":
                case "tex_reddirt_tiling":
                    SelectedFootsteps = DirtFootsteps;
                    break;
                case "tex_path":
                case "tex_pathnomoss":
                    SelectedFootsteps = ConcreteFootsteps;
                    break;
                case "tex_gravel_tiling":
                    SelectedFootsteps = GravelFootsteps;
                    break;
                case "tex_grass":
                    SelectedFootsteps = GrassFootsteps;
                    break;
                default:
                    SelectedFootsteps = DefaultFootsteps;
                    //Debug.Log("Unknown terrain texture, using default footsteps.");
                    break;
            }
        }
        else
        {
            SelectedFootsteps = DefaultFootsteps;
            //Debug.Log("Invalid texture index, using default footsteps.");
        }
    }

    void AssignFootstepSound(string textureName)
    {
        switch (textureName)
        {
            case "tex_reccenterinterior_base":
            case "gh_interiorframe_basemap":
            case "ehq_interior_basemap":
            case "tex_powerctrlinterior":
            case "tex_scientisthouseint":
                SelectedFootsteps = ConcreteFootsteps;
                break;
            case "tex_dirt":
            case "tex_dirt_grey":
            case "tex_dirt_lightbrown":
            case "tex_reddirt_tiling":
                SelectedFootsteps = DirtFootsteps;
                break;
            case "tex_path":
            case "tex_pathnomoss":
                SelectedFootsteps = ConcreteFootsteps;
                break;
            case "tex_gravel_tiling":
                SelectedFootsteps = GravelFootsteps;
                break;
            case "tex_grass":
                SelectedFootsteps = GrassFootsteps;
                break;
            default:
                SelectedFootsteps = DefaultFootsteps;
                //Debug.Log("Unknown terrain texture, using default footsteps.");
                break;
        }
    }


    public void PlayOneShot()
    {
        if (SelectedFootsteps.Guid != System.Guid.Empty)
        {
            RuntimeManager.PlayOneShot(SelectedFootsteps, transform.position);
        }
        else
        {
            RuntimeManager.PlayOneShot(DefaultFootsteps, transform.position);
        }
    }
}

