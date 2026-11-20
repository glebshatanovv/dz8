
using System;
using System.Collections;
using System.IO;

enum AccountType
{
    Текущий,
    Сберегательный
}

class BankTransaction
{
    private readonly DateTime date;
    private readonly double amount;

    public BankTransaction(double sum)
    {
        date = DateTime.Now;
        amount = sum;
    }

    public override string ToString()
    {
        return $"Дата: {date}, Сумма: {amount} рублей";
    }
}

class BankAccount : IDisposable
{
    private static int lastNumber = 1000;

    private Queue transactions = new Queue();
    private int accountNumber;
    private double balance;
    private AccountType type;

    static void Main()
    {
        BankAccount account1 = new BankAccount(
            5000, AccountType.Сберегательный);

        BankAccount account2 = new BankAccount(
            2000, AccountType.Текущий);

        Console.WriteLine("\nПервый счет:");
        account1.PrintInfo();

        Console.WriteLine("\nВторой счет:");
        account2.PrintInfo();

        Console.WriteLine("\nПосле перевода:");
        account1.Transfer(account2, 1000);

        Console.WriteLine("\nПервый счет:");
        account1.PrintInfo();

        Console.WriteLine("\nВторой счет:");
        account2.PrintInfo();

        // Создание третьего счета
        using (BankAccount account = new BankAccount(
            5000, AccountType.Сберегательный))
        {
            Console.WriteLine("\nТретий счет:");
            account.PrintInfo();

            account.Deposit(1000);
            account.Withdraw(2000);

            Console.WriteLine("\nПосле операций:");
            account.PrintInfo();
        }

        // Сохраняем операции первых двух счетов
        account1.Dispose();
        account2.Dispose();

        Console.WriteLine("\nПроводки записаны в файл.");
    }

    // Генерация уникального номера
    private static int GenerateNumber()
    {
        lastNumber++;
        return lastNumber;
    }

    // Конструкторы
    public BankAccount(double balance)
    {
        accountNumber = GenerateNumber();
        this.balance = balance;
        type = AccountType.Текущий;
    }

    public BankAccount(AccountType type)
    {
        accountNumber = GenerateNumber();
        balance = 0;
        this.type = type;
    }

    public BankAccount(double balance, AccountType type)
    {
        accountNumber = GenerateNumber();
        this.balance = balance;
        this.type = type;
    }

    public BankAccount()
    {
        accountNumber = GenerateNumber();
        balance = 0;
        type = AccountType.Текущий;
    }

    public int AccountNumber
    {
        get { return accountNumber; }
    }

    public double Balance
    {
        get { return balance; }
    }

    public AccountType Type
    {
        get { return type; }
        set { type = value; }
    }

    // Вывод информации о счете
    public void PrintInfo()
    {
        Console.WriteLine($"\nНомер счета: {accountNumber}");
        Console.WriteLine($"Баланс: {balance}");
        Console.WriteLine($"Тип счета: {type}");
    }

    // Пополнение счета
    public void Deposit(double sum)
    {
        if (sum > 0)
        {
            balance += sum;

            BankTransaction transaction =
                new BankTransaction(sum);

            transactions.Enqueue(transaction);

            Console.WriteLine(
                $"Пополнение на {sum} рублей выполнено.");
        }
        else
        {
            Console.WriteLine("Некорректная сумма.");
        }
    }

    // Снятие денег
    public void Withdraw(double sum)
    {
        if (sum > 0 && sum <= balance)
        {
            balance -= sum;

            BankTransaction transaction =
                new BankTransaction(-sum);

            transactions.Enqueue(transaction);

            Console.WriteLine($"Снято {sum} рублей.");
        }
        else
        {
            Console.WriteLine(
                "Недостаточно средств или некорректная сумма.");
        }
    }

    // Перевод денег на другой счет
    public void Transfer(BankAccount account, double sum)
    {
        if (account != null && sum > 0 && sum <= balance)
        {
            balance -= sum;
            account.balance += sum;

            BankTransaction transaction1 =
                new BankTransaction(-sum);

            transactions.Enqueue(transaction1);

            BankTransaction transaction2 =
                new BankTransaction(sum);

            account.transactions.Enqueue(transaction2);

            Console.WriteLine(
                $"Перевод {sum} рублей выполнен успешно.");
        }
        else
        {
            Console.WriteLine(
                "Ошибка перевода: недостаточно средств или некорректная сумма.");
        }
    }

    // Запись проводок в файл
    public void Dispose()
    {
        using (StreamWriter writer =
               new StreamWriter("transactions.txt", true))
        {
            while (transactions.Count > 0)
            {
                BankTransaction transaction =
                    (BankTransaction)transactions.Dequeue();

                writer.WriteLine(transaction);
            }
        }

        GC.SuppressFinalize(this);
    }
}
