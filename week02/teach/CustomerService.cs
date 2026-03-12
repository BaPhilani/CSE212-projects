/// <summary>
/// Maintain a Customer Service Queue.  Allows new customers to be 
/// added and allows customers to be serviced.
/// </summary>
public class CustomerService {
    public static void Run() {
        var originalIn = Console.In;
        var originalOut = Console.Out;

        string CaptureOutput(Action action, string input = "") {
            var previousIn = Console.In;
            var previousOut = Console.Out;
            Console.SetIn(new StringReader(input));
            var writer = new StringWriter();
            Console.SetOut(writer);
            try {
                action();
                return writer.ToString();
            }
            finally {
                Console.SetIn(previousIn);
                Console.SetOut(previousOut);
            }
        }

        void RestoreConsole() {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }

        void Report(int testNumber, string scenario, bool passed) {
            Console.WriteLine($"Test {testNumber}: {scenario}");
            Console.WriteLine($"Result: {(passed ? "PASS" : "FAIL")}");
            Console.WriteLine("=================");
        }

        try {
            // Test 1
            // Scenario: Valid max queue size is provided.
            // Expected Result: Queue reports the provided max size.
            var service = new CustomerService(4);
            Report(1, "Valid queue size is used", service.ToString().Contains("max_size=4"));

            // Test 2
            // Scenario: Invalid max queue size is provided.
            // Expected Result: Queue defaults max size to 10.
            service = new CustomerService(0);
            Report(2, "Invalid queue size defaults to 10", service.ToString().Contains("max_size=10"));

            // Test 3
            // Scenario: Add one customer.
            // Expected Result: Customer record is enqueued.
            service = new CustomerService(3);
            CaptureOutput(() => service.AddNewCustomer(), "Alice\nA100\nBilling issue\n");
            var queueAfterAdd = service.ToString();
            var addPassed = queueAfterAdd.Contains("size=1") &&
                            queueAfterAdd.Contains("Alice (A100)  : Billing issue");
            Report(3, "AddNewCustomer enqueues a customer", addPassed);

            // Test 4
            // Scenario: Add a customer when queue is full.
            // Expected Result: Error is displayed and customer is not added.
            service = new CustomerService(1);
            CaptureOutput(() => service.AddNewCustomer(), "Bob\nB200\nPassword reset\n");
            var fullQueueOutput = CaptureOutput(() => service.AddNewCustomer(), "Carol\nC300\nLocked account\n");
            var queueAfterFull = service.ToString();
            var fullPassed = fullQueueOutput.Contains("Maximum Number of Customers in Queue.") &&
                             queueAfterFull.Contains("size=1") &&
                             !queueAfterFull.Contains("Carol (C300)");
            Report(4, "Full queue shows error on add", fullPassed);

            // Test 5
            // Scenario: Serve next customer from a non-empty queue.
            // Expected Result: The next customer is displayed and dequeued.
            service = new CustomerService(3);
            CaptureOutput(() => service.AddNewCustomer(), "Diana\nD400\nPayment failed\n");
            CaptureOutput(() => service.AddNewCustomer(), "Evan\nE500\nAddress change\n");
            var serveOutput = CaptureOutput(() => service.ServeCustomer());
            var queueAfterServe = service.ToString();
            var servePassed = serveOutput.Contains("Diana (D400)  : Payment failed") &&
                              queueAfterServe.Contains("size=1") &&
                              queueAfterServe.Contains("Evan (E500)  : Address change") &&
                              !queueAfterServe.Contains("Diana (D400)");
            Report(5, "ServeCustomer dequeues and displays next customer", servePassed);

            // Test 6
            // Scenario: Serve customer from an empty queue.
            // Expected Result: Error is displayed.
            service = new CustomerService(2);
            var emptyServeOutput = CaptureOutput(() => service.ServeCustomer());
            Report(6, "Empty queue shows error on serve", emptyServeOutput.Contains("No Customers in the queue"));
        }
        finally {
            RestoreConsole();
        }
    }

    private readonly List<Customer> _queue = new();
    private readonly int _maxSize;

    public CustomerService(int maxSize) {
        if (maxSize <= 0)
            _maxSize = 10;
        else
            _maxSize = maxSize;
    }

    /// <summary>
    /// Defines a Customer record for the service queue.
    /// This is an inner class.  Its real name is CustomerService.Customer
    /// </summary>
    private class Customer {
        public Customer(string name, string accountId, string problem) {
            Name = name;
            AccountId = accountId;
            Problem = problem;
        }

        private string Name { get; }
        private string AccountId { get; }
        private string Problem { get; }

        public override string ToString() {
            return $"{Name} ({AccountId})  : {Problem}";
        }
    }

    /// <summary>
    /// Prompt the user for the customer and problem information.  Put the 
    /// new record into the queue.
    /// </summary>
    private void AddNewCustomer() {
        // Verify there is room in the service queue
        if (_queue.Count >= _maxSize) {
            Console.WriteLine("Maximum Number of Customers in Queue.");
            return;
        }

        Console.Write("Customer Name: ");
        var name = Console.ReadLine()!.Trim();
        Console.Write("Account Id: ");
        var accountId = Console.ReadLine()!.Trim();
        Console.Write("Problem: ");
        var problem = Console.ReadLine()!.Trim();

        // Create the customer object and add it to the queue
        var customer = new Customer(name, accountId, problem);
        _queue.Add(customer);
    }

    /// <summary>
    /// Dequeue the next customer and display the information.
    /// </summary>
    private void ServeCustomer() {
        if (_queue.Count <= 0) {
            Console.WriteLine("No Customers in the queue");
            return;
        }

        var customer = _queue[0];
        _queue.RemoveAt(0);
        Console.WriteLine(customer);
    }

    /// <summary>
    /// Support the WriteLine function to provide a string representation of the
    /// customer service queue object. This is useful for debugging. If you have a 
    /// CustomerService object called cs, then you run Console.WriteLine(cs) to
    /// see the contents.
    /// </summary>
    /// <returns>A string representation of the queue</returns>
    public override string ToString() {
        return $"[size={_queue.Count} max_size={_maxSize} => " + string.Join(", ", _queue) + "]";
    }
}