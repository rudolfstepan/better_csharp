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
