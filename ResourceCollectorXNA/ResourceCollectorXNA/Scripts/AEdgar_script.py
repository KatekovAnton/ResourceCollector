if pack == Null:
    FormMainPackExplorer.Instance.add_pack("D:\\projects\\ULJANIK493DEMO\\PhysX test2\\PhysX test2\\Data\\Ship.pack")
Eggs.ClearPack()
FormMainPackExplorer.Instance.import_pack_all("C:\\Users\\shpengler\\Desktop\\Ship.pack")
FormMainPackExplorer.Instance.add_new_textures_from_path("C:\\Users\\Public\\uljanik493\\textures_to_pack")

pack.Attach(Eggs.CreateCollisionMeshes())
pack.Attach(Eggs.CreateRenderObjectDescriptions())
pack.Attach(Eggs.CreateDiffuseMaterials())
pack.Attach(Eggs.CreateLevelObjectDescriptions())
rename(__LevelObjectDescription,"^lo_.+","lo\c",True)

#File.Copy("D:\\projects\\ULJANIK493DEMO\\PhysX test2\\PhysX test2\\Data\\eggs.cfg","D:\\projects\\ULJANIK493DEMO\\PhysX test2\\PhysX test2\\bin\\x86\\Debug\\eggs.cfg");

update()