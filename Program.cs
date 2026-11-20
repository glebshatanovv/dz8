
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManager
{
    // Статусы проекта
    enum ProjectStatus
    {
        Project,
        Execution,
        Closed
    }

    // Статусы задачи
    enum TaskStatus
    {
        Assigned,
        InProgress,
        Review,
        Completed
    }

    // Класс человека
    class Person
    {
        public string Name { get; set; }

        public Person(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    // Класс отчета
    class Report
    {
        public string Text { get; set; }
        public DateTime CompletionDate { get; set; }
        public Person Executor { get; set; }

        public Report(string text, Person executor)
        {
            Text = text;
            Executor = executor;
            CompletionDate = DateTime.Now;
        }

        public void Update(string text)
        {
            Text = text;
            CompletionDate = DateTime.Now;
        }

        public void Print()
        {
            Console.WriteLine("Текст отчета: " + Text);
            Console.WriteLine("Дата выполнения: " + CompletionDate);
            Console.WriteLine("Исполнитель: " + Executor.Name);
        }
    }

    // Класс задачи
    class Task
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }

        public Person Initiator { get; set; }
        public Person Executor { get; set; }

        public TaskStatus Status { get; set; }

        public Report TaskReport { get; set; }

        public Task(int number, string description,
                    DateTime startDate, DateTime deadline,
                    Person initiator, Person executor)
        {
            Number = number;
            Description = description;
            StartDate = startDate;
            Deadline = deadline;
            Initiator = initiator;
            Executor = executor;
            Status = TaskStatus.Assigned;
            TaskReport = null;
        }

        // Принять задачу
        public void Accept()
        {
            Status = TaskStatus.InProgress;
            Console.WriteLine("Задача принята в работу.");
        }

        // Делегировать задачу
        public void Delegate(Person newExecutor)
        {
            Executor = newExecutor;
            Status = TaskStatus.Assigned;

            Console.WriteLine("Задача передана: " + newExecutor.Name);
        }

        // Отклонить задачу
        public void Reject()
        {
            Executor = null;
            Status = TaskStatus.Assigned;

            Console.WriteLine("Задача отклонена.");
        }

        // Создать или обновить отчет
        public void CreateReport(string text)
        {
            if (TaskReport == null)
            {
                TaskReport = new Report(text, Executor);
            }
            else
            {
                TaskReport.Update(text);
            }

            Status = TaskStatus.Review;

            Console.WriteLine("Отчет отправлен инициатору на проверку.");
        }

        // Утвердить отчет
        public void ApproveReport()
        {
            Status = TaskStatus.Completed;
            Console.WriteLine("Отчет утвержден. Задача выполнена.");
        }

        // Вернуть отчет на доработку
        public void ReturnReport()
        {
            Status = TaskStatus.InProgress;
            Console.WriteLine("Отчет возвращен на доработку.");
        }

        public void Print()
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Номер задачи: " + Number);
            Console.WriteLine("Описание: " + Description);
            Console.WriteLine("Сроки: " + StartDate.ToShortDateString()
                              + " - " + Deadline.ToShortDateString());
            Console.WriteLine("Инициатор: " + Initiator.Name);

            Console.WriteLine("Исполнитель: " +
                (Executor == null ? "Не назначен" : Executor.Name));

            Console.WriteLine("Статус: " + Status);

            if (TaskReport != null)
            {
                Console.WriteLine("Отчет:");
                TaskReport.Print();
            }

            Console.WriteLine("--------------------------------");
        }
    }

    // Класс проекта
    class Project
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }

        public Person Initiator { get; set; }
        public Person TeamLead { get; set; }

        public List<Task> Tasks { get; set; }

        public ProjectStatus Status { get; set; }

        public Project(string description,
                       DateTime startDate,
                       DateTime deadline,
                       Person initiator,
                       Person teamLead)
        {
            Description = description;
            StartDate = startDate;
            Deadline = deadline;
            Initiator = initiator;
            TeamLead = teamLead;

            Tasks = new List<Task>();
            Status = ProjectStatus.Project;
        }

        // Создание задачи
        public void AddTask(Task task, Person creator)
        {
            if (Status != ProjectStatus.Project)
            {
                Console.WriteLine("Нельзя создавать задачи: проект уже запущен.");
                return;
            }

            if (creator != TeamLead)
            {
                Console.WriteLine("Только тимлид может создавать задачи.");
                return;
            }

            Tasks.Add(task);
            Console.WriteLine("Задача " + task.Number + " создана.");
        }

        // Удаление отклоненной задачи
        public void RemoveTask(Task task, Person creator)
        {
            if (creator != TeamLead)
            {
                Console.WriteLine("Удалить задачу может только тимлид.");
                return;
            }

            if (task.Executor != null)
            {
                Console.WriteLine("Сначала необходимо отклонить задачу.");
                return;
            }

            Tasks.Remove(task);
            Console.WriteLine("Задача удалена.");
        }

        // Запуск проекта
        public void StartExecution()
        {
            if (Tasks.Count == 0)
            {
                Console.WriteLine("Нельзя запустить проект без задач.");
                return;
            }

            Status = ProjectStatus.Execution;
            Console.WriteLine("Проект переведен в статус Исполнение.");
        }

        // Проверка выполнения всех задач
        public void CheckCompletion()
        {
            if (Status == ProjectStatus.Execution &&
                Tasks.Count > 0 &&
                Tasks.All(t => t.Status == TaskStatus.Completed))
            {
                Status = ProjectStatus.Closed;
                Console.WriteLine();
                Console.WriteLine("Все задачи выполнены!");
                Console.WriteLine("Проект закрыт.");
            }
        }

        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine("========== ПРОЕКТ ==========");
            Console.WriteLine("Описание: " + Description);
            Console.WriteLine("Начало: " + StartDate.ToShortDateString());
            Console.WriteLine("Окончание: " + Deadline.ToShortDateString());
            Console.WriteLine("Заказчик: " + Initiator.Name);
            Console.WriteLine("Тимлид: " + TeamLead.Name);
            Console.WriteLine("Статус проекта: " + Status);
            Console.WriteLine("Количество задач: " + Tasks.Count);
            Console.WriteLine("============================");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Создаем команду из 10 человек
            List<Person> team = new List<Person>()
            {
                new Person("Анна (заказчик)"),
                new Person("Иван (тимлид)"),
                new Person("Алексей"),
                new Person("Мария"),
                new Person("Дмитрий"),
                new Person("Екатерина"),
                new Person("Сергей"),
                new Person("Ольга"),
                new Person("Андрей"),
                new Person("Наталья")
            };

            Person customer = team[0];
            Person teamLead = team[1];

            // Создаем проект
            Project project = new Project(
                "Разработка веб-приложения",
                DateTime.Today,
                DateTime.Today.AddMonths(3),
                customer,
                teamLead
            );

            // Создаем задачи.
            // Каждый из 10 человек получает минимум одну задачу.

            string[] descriptions =
            {
                "Разработать техническое задание",
                "Создать дизайн интерфейса",
                "Разработать базу данных",
                "Создать страницу авторизации",
                "Разработать серверную часть",
                "Создать пользовательский интерфейс",
                "Настроить API",
                "Провести тестирование",
                "Исправить найденные ошибки",
                "Подготовить проект к запуску"
            };

            for (int i = 0; i < 10; i++)
            {
                Task task = new Task(
                    i + 1,
                    descriptions[i],
                    DateTime.Today,
                    DateTime.Today.AddDays(14),
                    teamLead,
                    team[i]
                );

                project.AddTask(task, teamLead);
            }

            // Запускаем проект после создания всех задач
            project.StartExecution();

            // Основной рабочий процесс
            while (project.Status != ProjectStatus.Closed)
            {
                project.Print();

                Console.WriteLine();
                Console.WriteLine("СПИСОК ЗАДАЧ:");

                foreach (Task task in project.Tasks)
                {
                    task.Print();
                }

                // Выбираем первую незавершенную задачу
                Task currentTask = project.Tasks.FirstOrDefault(
                    t => t.Status != TaskStatus.Completed
                );

                if (currentTask == null)
                {
                    project.CheckCompletion();
                    break;
                }

                // Если задача отклонена и не имеет исполнителя
                if (currentTask.Executor == null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Задача №" + currentTask.Number);
                    Console.WriteLine(currentTask.Description);

                    Console.WriteLine("1 - Назначить задачу");
                    Console.WriteLine("2 - Удалить задачу");

                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Console.WriteLine("Выберите исполнителя:");

                        for (int i = 0; i < team.Count; i++)
                        {
                            Console.WriteLine((i + 1) + " - " + team[i].Name);
                        }

                        int index;

                        if (int.TryParse(Console.ReadLine(), out index)
                            && index >= 1 && index <= team.Count)
                        {
                            currentTask.Delegate(team[index - 1]);
                        }
                        else
                        {
                            Console.WriteLine("Неверный номер.");
                        }
                    }
                    else if (choice == "2")
                    {
                        project.RemoveTask(currentTask, teamLead);
                    }

                    continue;
                }

                // Исполнитель получает задачу
                if (currentTask.Status == TaskStatus.Assigned)
                {
                    Console.WriteLine();
                    Console.WriteLine("ЗАДАЧА №" + currentTask.Number);
                    Console.WriteLine(currentTask.Description);
                    Console.WriteLine("Исполнитель: " + currentTask.Executor.Name);

                    Console.WriteLine("Выберите действие:");
                    Console.WriteLine("1 - Принять задачу");
                    Console.WriteLine("2 - Делегировать задачу");
                    Console.WriteLine("3 - Отклонить задачу");

                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        currentTask.Accept();
                    }
                    else if (choice == "2")
                    {
                        Console.WriteLine("Выберите нового исполнителя:");

                        for (int i = 0; i < team.Count; i++)
                        {
                            Console.WriteLine((i + 1) + " - " + team[i].Name);
                        }

                        int index;

                        if (int.TryParse(Console.ReadLine(), out index)
                            && index >= 1 && index <= team.Count
                            && team[index - 1] != currentTask.Executor)
                        {
                            currentTask.Delegate(team[index - 1]);
                        }
                        else
                        {
                            Console.WriteLine("Неверный выбор.");
                        }
                    }
                    else if (choice == "3")
                    {
                        currentTask.Reject();
                    }
                }

                // Исполнитель выполняет задачу и создает отчет
                else if (currentTask.Status == TaskStatus.InProgress)
                {
                    Console.WriteLine();
                    Console.WriteLine("ЗАДАЧА №" + currentTask.Number);
                    Console.WriteLine(currentTask.Description);
                    Console.WriteLine("Исполнитель: " + currentTask.Executor.Name);

                    Console.WriteLine("Введите отчет о выполненной работе:");

                    string reportText = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(reportText))
                    {
                        Console.WriteLine("Отчет не может быть пустым.");
                        continue;
                    }

                    currentTask.CreateReport(reportText);
                }

                // Инициатор проверяет отчет
                else if (currentTask.Status == TaskStatus.Review)
                {
                    Console.WriteLine();
                    Console.WriteLine("ОТЧЕТ НА ПРОВЕРКЕ");
                    Console.WriteLine("Задача №" + currentTask.Number);
                    Console.WriteLine(currentTask.Description);

                    currentTask.TaskReport.Print();

                    Console.WriteLine();
                    Console.WriteLine("Инициатор: " + currentTask.Initiator.Name);

                    Console.WriteLine("1 - Утвердить отчет");
                    Console.WriteLine("2 - Вернуть на доработку");

                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        currentTask.ApproveReport();
                    }
                    else if (choice == "2")
                    {
                        currentTask.ReturnReport();
                    }
                }

                // Проверяем, выполнены ли все задачи
                project.CheckCompletion();
            }

            Console.WriteLine();
            Console.WriteLine("========== ИТОГ ==========");

            project.Print();

            Console.WriteLine("Выполненные задачи:");

            foreach (Task task in project.Tasks)
            {
                task.Print();
            }

            Console.WriteLine("Работа программы завершена.");
            Console.ReadKey();
        }
    }
}
