import clr
import sys

clr.AddReference("System.Windows.Forms")
from System.Windows.Forms import *  

clr.AddReference("ResourceCollectorXNA")
from ResourceCollectorXNA import *
from ResourceCollector import *
from System import *
from System.Math import *
from System.IO import *
# импортирование dll-ок
# sys.path.Add(GameConfiguration.AppPath)
# clr.AddReferenceToFile("some.dll")


# создание необходимых функций
# это похоже на враппер
# желательно использовать функции, объ€вленные здесь, так как возможно их прототипы будут изменены

def quest(obj) : return Eggs.Question(obj)
def mess(obj) : return Eggs.Message(obj)
def ofd() : return Eggs.ofd()
def wr(obj): Eggs.wr(obj.ToString())
def Ex(obj) : SE.Instance.Execute(obj)
def ExScript(obj) : SE.Instance.ExScript(obj)
def update() : FormMainPackExplorer.Instance.UpdateData()
def getobjects(format, search = "") : return Eggs.GetObjects(format,search)
def rename(format, search, replace, incfg = False) : Eggs.Rename(format,search,replace,incfg)

# константы и подсказки
ExScript("_helper")
Null = Eggs.NULL
pack = Null