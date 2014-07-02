// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the VNCMARSHALLCPP_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// VNCMARSHALLCPP_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef VNCMARSHALLCPP_EXPORTS
#define VNCMARSHALLCPP_API __declspec(dllexport)
#else
#define VNCMARSHALLCPP_API __declspec(dllimport)
#endif

// This class is exported from the VncMarshallCpp.dll
class VNCMARSHALLCPP_API CVncMarshallCpp {
public:
	CVncMarshallCpp(void);
	
	bool StartServer(int portNumber);
	bool StopServer();
};

//extern VNCMARSHALLCPP_API int nVncMarshallCpp;
//
//VNCMARSHALLCPP_API int fnVncMarshallCpp(void);

VNCMARSHALLCPP_API bool StartServer(int);
VNCMARSHALLCPP_API bool StopServer(void);