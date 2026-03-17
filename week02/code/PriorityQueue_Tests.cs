using Microsoft.VisualStudio.TestTools.UnitTesting;

// TODO Problem 2 - Write and run test cases and fix the code to match requirements.

[TestClass]
public class PriorityQueueTests
{
    [TestMethod]
    // Scenario: Enqueue four values with varying priorities and dequeue until empty.
    // Expected Result: Values are returned by highest priority first, regardless of enqueue order.
    // Defect(s) Found: The highest-priority search skipped the last element and dequeue did not remove returned items.
    public void TestPriorityQueue_1()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 1);
        priorityQueue.Enqueue("B", 3);
        priorityQueue.Enqueue("C", 2);
        priorityQueue.Enqueue("D", 4);

        Assert.AreEqual("D", priorityQueue.Dequeue());
        Assert.AreEqual("B", priorityQueue.Dequeue());
        Assert.AreEqual("C", priorityQueue.Dequeue());
        Assert.AreEqual("A", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Enqueue multiple values where several share the same highest priority.
    // Expected Result: Among tied highest-priority items, dequeue follows FIFO order.
    // Defect(s) Found: Tie handling used >=, which selected the most recently seen tied item instead of the earliest.
    public void TestPriorityQueue_2()
    {
        var priorityQueue = new PriorityQueue();
        priorityQueue.Enqueue("A", 5);
        priorityQueue.Enqueue("B", 5);
        priorityQueue.Enqueue("C", 1);
        priorityQueue.Enqueue("D", 5);

        Assert.AreEqual("A", priorityQueue.Dequeue());
        Assert.AreEqual("B", priorityQueue.Dequeue());
        Assert.AreEqual("D", priorityQueue.Dequeue());
        Assert.AreEqual("C", priorityQueue.Dequeue());
    }

    [TestMethod]
    // Scenario: Dequeue from an empty queue.
    // Expected Result: InvalidOperationException is thrown with the required message.
    // Defect(s) Found: None after fixes.
    public void TestPriorityQueue_EmptyQueueThrows()
    {
        var priorityQueue = new PriorityQueue();

        try
        {
            priorityQueue.Dequeue();
            Assert.Fail("Exception should have been thrown.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual("The queue is empty.", ex.Message);
        }
        catch (AssertFailedException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Assert.Fail(
                string.Format("Unexpected exception of type {0} caught: {1}",
                ex.GetType(), ex.Message)
            );
        }
    }
}