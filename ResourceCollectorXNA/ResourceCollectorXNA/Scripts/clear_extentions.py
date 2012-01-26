for i in range(objects.Count):
    name = objects[i].name
    j = name.IndexOf(".")
    if j>0: pack.rename(i, name.Substring(0,j))
update()