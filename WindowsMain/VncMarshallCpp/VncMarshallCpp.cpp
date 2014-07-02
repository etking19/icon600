// VncMarshallCpp.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "VncMarshallCpp.h"

#include "util/CommonHeader.h"
#include "util/winhdr.h"
#include "util/CommandLine.h"
#include "win-system/WinCommandLineArgs.h"

#include "tvnserver-app/TvnService.h"
#include "tvnserver-app/TvnServerApplication.h"
#include "tvnserver-app/QueryConnectionApplication.h"
#include "tvnserver-app/DesktopServerApplication.h"
#include "tvnserver-app/AdditionalActionApplication.h"
#include "tvnserver-app/ServiceControlApplication.h"
#include "tvnserver-app/ServiceControlCommandLine.h"
#include "tvnserver-app/QueryConnectionCommandLine.h"
#include "tvnserver-app/DesktopServerCommandLine.h"

#include "tvncontrol-app/ControlApplication.h"
#include "tvncontrol-app/ControlCommandLine.h"

#include "tvnserver/resource.h"
#include "tvnserver-app/CrashHook.h"
#include "tvnserver-app/NamingDefs.h"

#include "tvnserver-app/WinEventLogWriter.h"

//// This is an example of an exported variable
//VNCMARSHALLCPP_API int nVncMarshallCpp=0;
//
//// This is an example of an exported function.
//VNCMARSHALLCPP_API int fnVncMarshallCpp(void)
//{
//	return 42;
//}

// This is the constructor of a class that has been exported.
// see VncMarshallCpp.h for the class definition
CVncMarshallCpp::CVncMarshallCpp()
{
	return;
}

bool CVncMarshallCpp::StartServer(int portNumber)
{
	return true;
}

bool CVncMarshallCpp::StopServer()
{
	return true;
}