// stdafx.h : ��׼ϵͳ�����ļ��İ����ļ���
// ���Ǿ���ʹ�õ��������ĵ�
// �ض�����Ŀ�İ����ļ�
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>
#include <Windows.h>
#include <assert.h>
#include <iostream>
#include <string>

using namespace std;

// TODO: �ڴ˴����ó�����Ҫ������ͷ�ļ�

#pragma comment( lib, "GraphLib.lib" )
extern "C" __declspec(dllimport) bool MeasureString(int* width, int* height, WCHAR* fontName, BYTE fontCharset, int fontHeight, int fontSpace, WCHAR* str)
