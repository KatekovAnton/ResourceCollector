pattern = quest("Create Scene\n search pattern (RegEx):")
if pattern != "Cancel":
    pack.Attach(Eggs.CreateCollisionMeshes(pattern))
    pack.Attach(Eggs.CreateRenderObjectDescriptions(pattern))
    pack.Attach(Eggs.CreateDiffuseMaterials(pattern))
    pack.Attach(Eggs.CreateLevelObjectDescriptions(pattern))
    rename(__LevelObjectDescription,"^lo_.+","lo\c")
    update()
    
#rename(ElementType.PNGTexture, "mu190", "mu120")

