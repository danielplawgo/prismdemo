Aplikacji PrismDemo s³u¿y do testowania biblioteki Prism oraz ró¿nych koncepcji wdra¿anych póŸniej do aplikacji Pharmacy.

W pliku tym znajduje siê ogólny opis budowy aplikacji. W komentarzach w kodzie znajduj¹ siê szczegó³y implementacyjne poszczególnych elementów projektu.

W katalogu Libs znajduj¹ siê wszystkie dodatkowe biblioteki potrzebne do dzia³ania aplikacji.

Katalog src to katalog, w którym znajduje siê kod aplikacji.

Aplikacji jest zbudowana z kilku projektów, gdzie poszczególne projekty maj¹ nastêpuj¹ce znaczenie:

*Prism.Shell - jest to g³ówny projekt aplikacji, w którym znajduj¹ siê dwa podstawowe elementy: 
	Shell, czyli okno g³ówne aplikacji, w którym okreœlone s¹ regionu wykorzystywane przez modu³y do wyœwietlania swoich widoków. 
	Bootstrapper - klasa odpowiedzialna za proces uruchamiania aplikacji oraz inicjacjê modu³ów aplikacji.

*Prism.Infrastucture - w projekcie tym znajduj¹ siê elementy infrastrukturalne ca³ej aplikacji, takie jak ogólne interfejsy, klasy bazowego, wspólne wiadomoœci itp. Projekty modu³ów bazuj¹ na tym projekcie.

*Prism.Entities - w projekcie tym znajduje siê klasy encyjne wykorzystywane w aplikacji.

*Prism.Module1 - przyk³adowy modu³ aplikacji, który zawiera widoku, viewmodele itp.

*Prism.Module1Test - projekt testów dla modu³u Prism.Module1.

Analizê przyk³adu najlepiej zacz¹æ od projektu Prism.Shell, nastêpnie Prism.Module1 oraz Prism.Module1.Tests (przy okazji analizuj¹ kod pozosta³ych modu³ów).