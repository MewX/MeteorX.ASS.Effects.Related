// GraphLib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "windows.h"
#include "assert.h"
#include "stdio.h"

class XString
{
private:
	WCHAR* buf;
public:
	XString()
	{
		buf = new WCHAR[1];
		buf[0] = 0;
	}

	~XString() { delete buf; }

	void Append(WCHAR* s)
	{
		WCHAR* newbuf = new WCHAR[wcslen(buf) + wcslen(s) + 1];
		wcscpy(newbuf, buf);
		wcscat(newbuf, s);
		delete buf;
		buf = newbuf;
	}

	WCHAR* ToPS()
	{
		WCHAR* tmp = new WCHAR[wcslen(buf) + 1];
		wcscpy(tmp, buf);
		return tmp;
	}
};

extern "C" __declspec(dllexport) WCHAR* TestHelloWorld()
{
	WCHAR* buf = new WCHAR[100];
	wcscpy(buf, L"Hello World.");
	buf[12] = 0;
	return buf;
}

extern "C" __declspec(dllexport) bool MeasureString(int* width, int* height, WCHAR* fontName, BYTE fontCharset, int fontHeight, int fontSpace, WCHAR* str)
{
	/*
	lfCharSet:
	ANSI_CHARSET				BALTIC_CHARSET
	CHINESEBIG5_ CHARSET		DEFAULT_CHARSET
	EASTEUROPE_CHARSET			GB2312_ CHARSET
	GREEK_CHARSET				HANGUL_CHARSET
	MAC_CHARSET					OEM_CHARSET
	RUSSIAN_CHARSET				SHIFTJIS_CHARSET
	SYMBOL_CHARSET				TURKISH_CHARSET
	VIETNAMESE_CHARSET
	窗口的韩国语言版本：JOHAB_CHARSET
	窗口的中东语言版本：ARABIC_CHARSET	HEBREW_CHARSET
	窗口的泰国语言版本：THAI_CHARSET
	*/
	HDC dc = ::GetDC(0);
	LOGFONT logfont;
	memset(&logfont, 0, sizeof(LOGFONT));
	logfont.lfCharSet = fontCharset;
	logfont.lfHeight = fontHeight;
	logfont.lfQuality = ANTIALIASED_QUALITY;
	logfont.lfWeight = 0;
	wcscpy(logfont.lfFaceName, fontName);
	HFONT hfont = ::CreateFontIndirectW(&logfont);
	HGDIOBJ holdfont = ::SelectObject(dc, hfont);
	*width = 0;
	for (const WCHAR *p = str; *p; p++) {
		WCHAR buf[2];
		int buflen;
		buf[0] = *p;
		buf[1] = 0;
		SIZE sz;
		GetTextExtentPoint32W(dc, buf, 1, &sz);
		*width += sz.cx + fontSpace;
	}
	*width -= fontSpace;
	*height = fontHeight;
	return true;
}

extern "C" __declspec(dllexport) int IntAdd(int a, int b)
{
	return a + b;
}

extern "C" __declspec(dllexport) bool GetOutline(WCHAR* buf, int* buflen, WCHAR thisChar, WCHAR* fontName, BYTE fontCharset, int fontHeight, int fontWeight, short yOffset, short OX, short OY)
{
	HDC dc = ::GetDC(0);
	LOGFONT logfont;
	memset(&logfont, 0, sizeof(LOGFONT));
	logfont.lfCharSet = fontCharset;
	logfont.lfHeight = fontHeight;
	logfont.lfQuality = ANTIALIASED_QUALITY;
	logfont.lfWeight = fontWeight;
	wcscpy(logfont.lfFaceName, fontName);
	HFONT hfont = ::CreateFontIndirectW(&logfont);
	HGDIOBJ holdfont = ::SelectObject(dc, hfont);
	MAT2 mat2 = {{0,1}, {0,0}, {0,0}, {0,-1}};
	POINT buffer[1024];
	GLYPHMETRICS gm;
	DWORD returnBytes = GetGlyphOutlineW(dc, thisChar, GGO_BEZIER, &gm, sizeof(buffer), buffer, &mat2);
	if (returnBytes == GDI_ERROR) return false;
	TTPOLYGONHEADER* pTTPH = (TTPOLYGONHEADER*)buffer;
	assert(pTTPH->dwType == TT_POLYGON_TYPE);
	XString xs;
	WCHAR sbuf[1024];
	while (returnBytes > 0)
	{
		wsprintf(sbuf, L" m %d %d\0", OX + pTTPH->pfxStart.x.value, OY + yOffset + pTTPH->pfxStart.y.value);
		xs.Append(sbuf);
		TTPOLYCURVE* pCurrentCurve = (TTPOLYCURVE*)(pTTPH+1);
		int remainBytes = pTTPH->cb - sizeof(TTPOLYGONHEADER);
		while (remainBytes > 0)
		{
			switch (pCurrentCurve->wType)
			{
				case TT_PRIM_LINE:
					wsprintf(sbuf, L" l");
					xs.Append(sbuf);
					for (int i = 0; i < pCurrentCurve->cpfx; i++)
					{
						wsprintf(sbuf, L" %d %d", OX + pCurrentCurve->apfx[i].x.value, OY + yOffset + pCurrentCurve->apfx[i].y.value);
						xs.Append(sbuf);
					}
					break;
				case TT_PRIM_QSPLINE:
					{
						wsprintf(sbuf, L" b");
						xs.Append(sbuf);
						double x0 = pTTPH->pfxStart.x.value;
						double y0 = pTTPH->pfxStart.y.value;
						for (int i = 0; i < pCurrentCurve->cpfx; i += 2)
						{
							double x1 = x0 / 3.0 + pCurrentCurve->apfx[i].x.value * 2.0 / 3.0;
							double y1 = y0 / 3.0 + pCurrentCurve->apfx[i].y.value * 2.0 / 3.0;
							double x2 = pCurrentCurve->apfx[i].x.value * 2.0 / 3.0 + pCurrentCurve->apfx[i + 1].x.value / 3.0;
							double y2 = pCurrentCurve->apfx[i].y.value * 2.0 / 3.0 + pCurrentCurve->apfx[i + 1].y.value / 3.0;
							double x3 = pCurrentCurve->apfx[i + 1].x.value;
							double y3 = pCurrentCurve->apfx[i + 1].y.value;
							wsprintf(sbuf, L" %d %d %d %d %d %d", OX + (int)x1, OY + yOffset + (int)y1, OX + (int)x2, OY + yOffset + (int)y2, OX + (int)x3, OY + yOffset + (int)y3);
							xs.Append(sbuf);
							x0 = x3;
							y0 = y3;
						}
						break;
					}
				case TT_PRIM_CSPLINE:
					wsprintf(sbuf, L" b");
					xs.Append(sbuf);
					for (int i = 0; i < pCurrentCurve->cpfx; i++)
					{
						wsprintf(sbuf, L" %d %d", OX + pCurrentCurve->apfx[i].x.value, OY + yOffset + pCurrentCurve->apfx[i].y.value);
						xs.Append(sbuf);
					}
					break;
			}
			int count = sizeof(TTPOLYCURVE) + (pCurrentCurve->cpfx-1)*sizeof(POINTFX);
			pCurrentCurve = (TTPOLYCURVE*)((char*)pCurrentCurve + count);
			remainBytes -= count;
		}
		wsprintf(sbuf, L" c");
		xs.Append(sbuf);
		returnBytes -= pTTPH->cb;
		pTTPH = (TTPOLYGONHEADER*)((char*)pTTPH + pTTPH->cb);
	}
	WCHAR *result = xs.ToPS();
	wcscpy(buf, result);
	*buflen = (LONG)wcslen(result);
	return true;
}
