#include <Windows.h>
#include <stdio.h>

int main(void)
{
	HINSTANCE hDll;

	hDll = LoadLibrary(TEXT("n0isegat3.dll"));

	if (hDll != NULL) {
		printf("successfully found n0isegat3 dll");
	}
	else {
		printf("cannot find n0isegat3 dll");
	}

	return 0;
}