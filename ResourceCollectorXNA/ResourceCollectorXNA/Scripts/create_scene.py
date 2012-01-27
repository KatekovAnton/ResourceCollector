pattern = quest("Ineed search pattern (RegEx)")
if pattern != "Cancel":
    pack.Attach(Eggs.CreateCollisionMeshes(pattern))
    pack.Attach(Eggs.CreateRenderObjectDescriptions(pattern))
    pack.Attach(Eggs.CreateDiffuseMaterials(pattern))
    update()
    
#rename(ElementType.PNGTexture, "mu190", "mu120")

