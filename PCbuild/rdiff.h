#pragma once
#ifndef __RDIFF_H__
#define __RDIFF_H__

#ifdef BUILDDLL
#define DLLAPI __declspec(dllexport)
#else
#define DLLAPI __declspec(dllimport)
#endif

DLLAPI rs_result rdiff_sig(const char *baseFile, const char *sigFile);
DLLAPI rs_result rdiff_delta(const char *sigFile, const char *newFile, const char *deltaFile);
DLLAPI rs_result rdiff_patch(const char *baseFile, const char *deltaFile, const char *newFile);

#endif