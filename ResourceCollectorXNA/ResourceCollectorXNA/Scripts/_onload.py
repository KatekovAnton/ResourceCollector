import clr
import sys
clr.AddReference("ResourceCollectorXNA")
from ResourceCollectorXNA import *
from System import *
from System.Math import *

# �������������� dll-��
# sys.path.Add(GameConfiguration.AppPath)
# clr.AddReferenceToFile("some.dll")


# �������� ����������� �������
# ��� ������ �� �������
# ���������� ������������ �������, ����������� �����, ��� ��� �������� �� ��������� ����� ��������

def Ex(obj) : SE.Instance.Execute(obj)
def ExScript(obj) : SE.Instance.ExScript(obj)