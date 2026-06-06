using NUnit.Framework;
using Core.Algorithms;
using System;

namespace Core.Tests
{
    [TestFixture]
    public class AlgorithmTests
    {
        [Test]
        public void MultiwayMergeSort_ShouldSortArrayCorrectly()
        {
            // Arrange (Підготовка)
            int[] data = { 15, 2, 9, 1, 5, 6, 20, 3 };
            int[] expected = { 1, 2, 3, 5, 6, 9, 15, 20 };
            var sorter = new MultiwayMergeSortAlgorithm(3); // k = 3

            // Act (Дія)
            sorter.Execute(data);

            // Assert (Перевірка)
            Assert.That(data, Is.EqualTo(expected), "Масив має бути відсортований за зростанням.");
        }

        [Test]
        public void MultiwayMergeSort_EmptyArray_ShouldNotThrow()
        {
            // Arrange
            int[] data = Array.Empty<int>();
            var sorter = new MultiwayMergeSortAlgorithm();

            // Act & Assert
            Assert.DoesNotThrow(() => sorter.Execute(data), "Порожній масив не повинен викликати помилку.");
        }

        [Test]
        public void RedBlackTree_OrderStatistic_ShouldReturnCorrectRankAndElement()
        {
            // Arrange
            // Відсортований вигляд: 3, 6, 7, 9, 13, 15, 17, 18, 20
            int[] data = { 15, 6, 18, 3, 7, 17, 20, 9, 13 };
            var rbTree = new RedBlackTreeAlgorithm();

            // Act
            rbTree.Execute(data);

            // Assert
            // 4-й за величиною елемент має бути 9
            Assert.That(rbTree.OsSelect(4), Is.EqualTo(9), "OsSelect має повертати правильний елемент за його рангом.");
            
            // Ранг елемента 17 має бути 7
            Assert.That(rbTree.OsRank(17), Is.EqualTo(7), "OsRank має повертати правильний ранг елемента.");
        }

        [Test]
        public void BTree_Insertion_ShouldProcessWithoutExceptions()
        {
            // Arrange
            int[] data = { 10, 20, 5, 6, 12, 30, 7, 17 };
            var bTree = new BTreeAlgorithm();

            // Act & Assert
            // Перевіряємо інваріант: вставка валідних даних не повинна "ламати" дерево
            Assert.DoesNotThrow(() => bTree.Execute(data), "Вставка в B-Tree має проходити без помилок.");
        }

        [Test]
        public void MultiwayMergeSort_WithDuplicatesAndNegatives_ShouldSortCorrectly()
        {
            // Arrange: масив з від'ємними числами та дублікатами
            int[] data = { 5, -2, 9, 5, 0, -2, 10, 0 };
            int[] expected = { -2, -2, 0, 0, 5, 5, 9, 10 };
            var sorter = new MultiwayMergeSortAlgorithm(3);

            // Act
            sorter.Execute(data);

            // Assert
            Assert.That(data, Is.EqualTo(expected), "Алгоритм має правильно обробляти від'ємні числа та дублікати.");
        }

        [Test]
        public void MultiwayMergeSort_AlreadySortedArray_ShouldRemainSorted()
        {
            // Arrange: вже відсортований масив
            int[] data = { 1, 2, 3, 4, 5, 6, 7 };
            int[] expected = { 1, 2, 3, 4, 5, 6, 7 };
            var sorter = new MultiwayMergeSortAlgorithm();

            // Act
            sorter.Execute(data);

            // Assert
            Assert.That(data, Is.EqualTo(expected), "Вже відсортований масив не повинен ламатися.");
        }

        [Test]
        public void RedBlackTree_OsRank_MissingElement_ShouldReturnMinusOne()
        {
            // Arrange
            int[] data = { 10, 20, 30, 40, 50 };
            var rbTree = new RedBlackTreeAlgorithm();

            // Act
            rbTree.Execute(data);

            // Assert: шукаємо елемент, якого немає в дереві (наприклад, 99)
            Assert.That(rbTree.OsRank(99), Is.EqualTo(-1), "Якщо елемента немає в дереві, OsRank має повертати -1.");
        }

        [Test]
        public void BTree_Insertion_LargeDataScale_ShouldTriggerNodeSplitsSuccessfully()
        {
            // Arrange: генеруємо 1000 елементів у зворотному порядку, 
            // щоб спровокувати масове розбиття вузлів (SplitChild) у B-дереві
            int[] data = Enumerable.Range(1, 1000).Reverse().ToArray();
            var bTree = new BTreeAlgorithm();

            // Act & Assert
            Assert.DoesNotThrow(() => bTree.Execute(data), 
                "B-Tree має успішно масштабуватися та розбивати вузли при великій кількості даних.");
        }
    }
}