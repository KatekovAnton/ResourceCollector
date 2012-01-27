pattern = quest("Clear Pack....\n search pattern (RegEx):")
if pattern != "Cancel":
    Eggs.ClearPack(pattern)
    
update()