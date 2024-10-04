using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace QueueMarket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> marketProducts = ReadFile("DataBaseMarket.txt");
            Random random = new Random();
            Work(marketProducts, random);
        }

        static void Work(Dictionary<string, int> marketProducts, Random random)
        {
            Queue<List<string>> vouchers = new Queue<List<string>>();

            int maxShoppers = 15;
            int shoppers = random.Next(1, maxShoppers + 1);
            int amountPricesProducts;
            int money = 0;

            vouchers = FillQueueBuyers(marketProducts, shoppers, random);

            while (true)
            {
                Console.Clear();
                amountPricesProducts = GetAmountShoppingBasket(marketProducts, vouchers.Peek());
                DrawUserInterface(marketProducts, vouchers.Dequeue(), vouchers, amountPricesProducts, money);
                money += amountPricesProducts;
                Console.ReadKey();
            }
        }

        static Queue<List<string>> FillQueueBuyers(Dictionary<string, int> marketProducts, int shoppers, Random random)
        {
            Queue<List<string>> vouchers = new Queue<List<string>>();
            List<string> shoppingBasket = new List<string>();

            for (int i = 0; i < shoppers; i++)
            {
                shoppingBasket = FillShoppingBasket(marketProducts, random);
                vouchers.Enqueue(shoppingBasket);
            }

            return vouchers;
        }

        static void DrawUserInterface(Dictionary<string, int> marketProducts, List<string> shoppingBasket, Queue<List<string>> vouchers, int amountPricesProducts, int money)
        {
            ShowVoucher(marketProducts, shoppingBasket, amountPricesProducts);
            Console.SetCursorPosition(20, 1);
            Console.WriteLine($"В очереди сейчас: {vouchers.Count}");
            Console.SetCursorPosition(20, 0);
            Console.WriteLine($"Денег в кассе: {money}");
        }

        static void ShowVoucher(Dictionary<string, int> marketProducts, List<string> shoppingBasket, int amountPricesProducts)
        {
            Console.WriteLine("Корзина: ");

            for (int i = 0; i <= shoppingBasket.Count - 1; i++)
            {
                Console.Write($"{shoppingBasket[i]} - {marketProducts[shoppingBasket[i]]}\n");
            }

            Console.WriteLine($"Итого: {amountPricesProducts}");
        }

        static int GetAmountShoppingBasket(Dictionary<string, int> marketProducts, List<string> shoppingBasket)
        {
            int amountShoppingBasket = 0;

            for (int i = 0; i < shoppingBasket.Count; i++)
            {
                amountShoppingBasket += marketProducts[shoppingBasket[i]];
            }

            return amountShoppingBasket;
        }

        static List<string> FillShoppingBasket(Dictionary<string, int> pricesOnFood, Random random)
        {
            List<string> shoppingBasket = new List<string>();

            int minSizeBasket = 1;
            int maxSizeBasket = 10;
            int productsCount = random.Next(minSizeBasket, maxSizeBasket);

            for (int i = 0; i < productsCount; i++)
            {
                int productIndex = random.Next(pricesOnFood.Count); 
                string productName = pricesOnFood.ElementAt(productIndex).Key;
                shoppingBasket.Add(productName);
            }

            return shoppingBasket;
        }

        static Dictionary<string, int> ReadFile(string path)
        {
            string[] file = File.ReadAllLines(path);
            Dictionary<string, int> marketProducts = new Dictionary<string, int>(file.Length);

            char separator = ' ';

            foreach (string line in file)
            {
                string[] products = line.Split(separator);
                string nameProduct = products[0];
                int priceProduct = Convert.ToInt32(products[1]);

                marketProducts.Add(nameProduct, priceProduct);
            }

            return marketProducts;
        }
    }
}