import clr
import sys
clr.AddReference("ResourceCollectorXNA")
from ResourceCollectorXNA import *
from ResourceCollector import *
from System import *
from System.Math import *

# импортирование dll-ок
# sys.path.Add(GameConfiguration.AppPath)
# clr.AddReferenceToFile("some.dll")


# создание необходимых функций
# это похоже на враппер
# желательно использовать функции, объ€вленные здесь, так как возможно их прототипы будут изменены

def ofd() : return Eggs.ofd()
def wr(obj): ConsoleWindow.TraceMessage(obj.ToString())
def Ex(obj) : SE.Instance.Execute(obj)
def ExScript(obj) : SE.Instance.ExScript(obj)
def update() : FormMainPackExplorer.Instance.UpdateData()