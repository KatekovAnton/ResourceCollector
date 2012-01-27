update()

for i in range(pack.Objects.Count):
    name = pack.Objects[i].name
    j = name.IndexOf(".")
    if j>0: pack.rename(i, name.Substring(0,j) + "\0")
        
update()