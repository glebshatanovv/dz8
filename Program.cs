using System;
using System.Collections;
using System.Runtime.InteropServices.JavaScript;

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
}

class BankAccount
{
    private static int lastNumber = 1000;
    static void Main()
    {
            BankAccount account1 = new BankAccount(5000, AccountType.Сберегательный);

            BankAccount account2 = new BankAccount(2000, AccountType.Текущий);
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
           BankAccount account = new BankAccount(5000, AccountType.Сберегательный);
        account.PrintInfo();
        account.Deposit(1000);
        account.Withdraw(2000);
        account.PrintInfo();
    }
    private Queue transactions = new Queue();
    private int accountNumber;
    private double balance;
    private AccountType type;

    // Генерация уникального номера
    private static int GenerateNumber()
    {
        lastNumber++;
        return lastNumber;
    }
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

    // Конструктор
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
    // Чтение данных

    public void PrintInfo()
    {
        Console.WriteLine($"\nНомер счета: {accountNumber}");
        Console.WriteLine($"Баланс: {balance}");
        Console.WriteLine($"Тип счета: {type}");
    }

    // Положить деньги
    public void Deposit(double sum)
    {
        if (sum > 0)
        {
            balance += sum;
            
            Console.WriteLine($"Пополнение на {sum} рублей выполнено:");
            
            BankTransaction transaction = new BankTransaction(sum);
            transactions.Enqueue(transaction);
            
           
        }
        else
            Console.WriteLine("Некорректная сумма.");
    }

    // Снять деньги
    public void Withdraw(double sum)
    {
        if (sum > 0 && sum <= balance)
        {
            balance -= sum;

            Console.WriteLine($"Снято {sum} рублей:");
            
            BankTransaction transaction = new BankTransaction(sum);
            transactions.Enqueue(transaction);
            
        }
        else
            Console.WriteLine("Недостаточно средств или некорректная сумма.");
    }
    // Перевод денег с другого счета на текущий
    
        public void Transfer(BankAccount account, double sum)
        {
            if (account != null && sum > 0 && sum <= balance)
            {
                // Снимаем деньги с текущего счета
                balance -= sum;

                // Добавляем деньги на счет получателя
                account.balance += sum;

                // Записываем операцию снятия
                BankTransaction transaction1 = new BankTransaction(-sum);
                transactions.Enqueue(transaction1);

                // Записываем операцию пополнения
                BankTransaction transaction2 = new BankTransaction(sum);
                account.transactions.Enqueue(transaction2);

                Console.WriteLine($"Перевод {sum} рублей выполнен успешно.");
            }
            else
            {
                Console.WriteLine("Ошибка перевода: недостаточно средств или некорректная сумма.");
            }
        }
    }

    
