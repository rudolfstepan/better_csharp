# Outer and Inner Class Example

## Zweck

Dieses Projekt demonstriert die Verwendung von inneren Klassen in C#. Es zeigt, wie man von einer inneren Klasse auf Eigenschaften der äußeren Klasse zugreifen kann, ohne eine Instanz der äußeren Klasse explizit an die innere Klasse weiterzugeben. Stattdessen wird eine statische Instanz der äußeren Klasse verwendet.

## Codeübersicht

### OuterClass

Die `OuterClass` enthält eine öffentliche Zeichenfolgeneigenschaft `OuterClassName` und eine statische Instanz der Klasse selbst, `OuterClassInstance`.

```csharp
public class OuterClass
{
    public string OuterClassName = "OuterClass";

    // Static instance of the outer class
    public static OuterClass OuterClassInstance;
    
    // Static constructor to initialize the static instance
    static OuterClass()
    {
        OuterClassInstance = new OuterClass();
    }

    public class InnerClass
    {
        public void Test()
        {
            // Accessing the outer class's member using 'OuterClassInstance'
            Console.WriteLine(OuterClassInstance.OuterClassName);
            Console.WriteLine("Test");
        }
    }
}
```
Program
Die Program-Klasse erstellt eine Instanz der InnerClass und ruft deren Test-Methode auf, um zu demonstrieren, wie auf die Eigenschaft OuterClassName der äußeren Klasse zugegriffen wird.

```csharp
public class Program
{
    public static void Main()
    {
        OuterClass.InnerClass inner = new OuterClass.InnerClass();
        inner.Test();
    }
}
```

Verwendung
Klone das Repository:

bash
Code kopieren
git clone https://github.com/dein-benutzername/dein-repository.git
Öffne das Projekt in deiner bevorzugten IDE (z.B. Visual Studio).

Baue und führe das Projekt aus. Die Konsole sollte folgendes ausgeben:

Code kopieren
OuterClass
Test

Lizenz
Dieses Projekt ist unter der MIT-Lizenz lizenziert – siehe die LICENSE-Datei für Details.
