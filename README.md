# Optymalizator zadania programowania nieliniowego

Autorzy: Krzysztof Danielak (implementacja algorytmu ewolucyjnego) i Marcin Kisiel (wizualizacja)

## Opis projektu

Celem projektu było opracowanie aplikacji umożliwiającej rozwiązanie zadania opisanego poprzez zadanie programowania nieliniowego dla zmiennych wymiaru wektora x. W przypadku tego tematu należało stworzyć aplikację systemową implementującą algorytm ewolucji różnicowej DE oraz zrozumiały i interaktywny interfejs użytkownika.


Zadanie optymalizacji jest zadaniem programowania nieliniowego. Funkcja celu to $\min_{\forall x \in R^5} f(x)$, gdzie f(x) to funkcja nieliniowa niewypukła.
Wymiar zadania n $\leq$ 5. Zadanie nie ma ograniczeń. Przyjęto metodę ewolucji różnicowej. Parametrem wejściowym jest liczba iteracji L. Należy także dobrać wzór na różnicową postać wektora.

## Możliwości użytkownika
Po uruchomieniu aplikacji pojawi się okno. Większą jego część zajmuje obszar do wyświetlania wykresów (jeśli przestrzeń jest możliwa do przedstawienia) oraz, jeśli zwiększymy rozmiar przestrzeni, pojawią się nowe pola, w których możemy wybrać zakres dla każdego parametru oraz sprawdzić najlepszy wynik dla każdej generacji. 

Po prawej stronie okna znajduje się panel sterowania algorytmem. Na samej górze mamy do wyboru przykładowe funkcje.

Poniżej znajduje się pole do wpisywania funkcji, dla której mamy przeprowadzić obliczenia. Jednocześnie przy wybraniu początkowej funkcji przy pomocy przycisków typu radio, wyświetla aktualnie wybraną funkcje, która jest przekazywana do globalnej zmiennej „polynomial”, na podstawie której przeprowadzane są obliczenia.
Zasady wprowadzania danych do pola funkcji są następujące:
- każda kolejna zmienna powinna zostać oznaczony indeksem następującym w kolejności rosnącej,
- parametry muszą być oznaczone literą „x” oraz indeksem np. x1, x2,
- przy mnożeniu parametru przez zmienną nie może być ona dopisana do zmiennej, lecz musi być oddzielona znakiem mnożenia „∗”,
- w polu nie mogą pojawić się żadne znaki poza cyframi, literą x, oraz oznaczeniami funkcji trygonometrycznych, wykładniczych, logarytmicznych,
- pole nie może być puste.
Jeśli funkcja zawiera dwie lub mniej zmiennych, ich zakresy mogą być podane od razu w głównym panelu sterowania.


**Wielkość populacji**

Wielkość populacji musi być całkowitą dodatnią wartością. Nie powinna być zbyt mała, aby zagwarantować różnorodność rozwiązańw populacji.


**Ilość generacji**

W polu rozdzielczości możemy podać jakoś z jaką wyświetlaną są płaszczyzny. Zwiększenie tej wartości prowadzi to dłuższego czasu generacji wykresu zaś zmniejszenie go może prowadzić do złego przedstawienia płaszczyzny oraz dużej rozbieżności między punktami populacji a przedstawionym torem funkcji.


**Prawdopodobieństwo krzyżowania**

Współczynnik opisuje prawdopodobieństwo wymiany elementów wektora, a zatem przekazania mutacji osobnikowi potomnemu. Współczynnik ten ma oznaczenie $C_r$ i ponieważ opisuje prawdopodobieństwo, to może przyjmować wartości z zakresu [0,1]. Istnieją funkcje, dla których niski spółczynnik prawdopodobieństwa jest  najbardziej efektywny. Storn i Price wykazali w swoich badaniach, że wszystkie zadania optymalizacji mogą być rozwiązane, gdy 0  $\leq$ C_r $\leq$ 0.2 dla funkcji, dla których dekompozycja jest możliwa lub 0 $\leq$ C_r $\leq$ 1. W ogólnym przypadku zalecane jest, aby $C_r$ było bliskie 1. W ten sposób straty wydajności związane z niską częstotliwością mutacji w populacjach potomnych są minimalizowane.

**Pozostałe opcje**

W polu rozdzielczości możemy podać jakość z jaką wyświetlaną są płaszczyzny. Zwiększenie tej wartości prowadzi to dłuższego czasu generacji wykresu zaś zmniejszenie go może prowadzić do złego przedstawienia płaszczyzny oraz dużej rozbieżności między punktami populacji a przedstawionym torem funkcji.

Jedną z możliwości podczas początkowego tworzenia populacji jest wybranie punktu, a raczej zakresu w jakim elementy tablicy mają zostać wygenerowane. Po zaznaczeniu pola checkbox możemy wybrać „promień”, który tak naprawdę jest maksymalną odległością (w każdym kierunku) od punktu w jakim może powstać członek populacji. Dla wszystkich zmiennych zastosowana jest jedna odległość R. 

Po podaniu funkcji i przeprowadzeniu obliczeń w polu „Optymalna wartość funkcji” pojawi się minimum funkcji. Pole „X1 i X2 wyniku” to współrzędne dla punktu który algorytm uznał za minimum.  

Ponad przyciskami do wykonania obliczeń oraz wyświetleniem płaszczyzny znajduje się suwak, na którym możemy wybrać generację którą chcemy wyświetlić. Cała populacja jest przedstawiana przy pomocy czerwonych sfer zaś najlepsze rozwiązanie dla danej generacji jest zaznaczane na różowo oraz sfera jest trzykrotnie powiększona dla łatwiejszego znalezienia na wykresie. Funkcjonalność ta została dodana w celu ilustracji kolejnych iteracji algorytmu oraz wyszczególnienia istotnych danych.  

Jednak aby nie ograniczać się do szczegółowej analizy, możemy wyświetlić wszystkie punkty jak i wszystkie schować, przy pomocy przycisków „Pokaż wszystkie pkt.” I „Usuń wszystkie pkt”. Dodatkowo bez przeprowadzania obliczeń możemy narysować żądaną płaszczyznę podaną w polu funkcji.

Niestety, w bibliotece nzy3D nie można rzutować warstwic na powierzchnię poniżej wykresu. Można natomiast obliczyć wartości funkcji a następnie wyświetlić płaszczyznę z zasięgiem osi Z przeskalowanej, aby spłaszczyć wykres. Dodatkowo na takiej mapie wyświetlane jest minimum funkcji.
