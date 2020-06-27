# -*- coding: utf-8 -*-

import os
import pathlib
import json
import clr  # pip install pythonnet

from pprint import pprint

clr.AddReference('Win10Ocr')
cwd=pathlib.Path(__file__).absolute().parent

from Win10Ocr import Win10UWPOcr

imageFile= cwd.parent /"Win10OcrTests/1.PNG"


engine=Win10UWPOcr()

with open(imageFile,'rb') as f:
    data=f.read()
    res=engine.RecognizeJsonStringLine(data)    # pass bytes to C#
resd=json.loads(res)
pprint(resd)


res=engine.RecognizeJsonStringLine(str(imageFile))  #pass str to C#
print(res)
