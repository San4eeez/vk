using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Model.Keyboard;

class Program
{
    // Хранение последних обработанных сообщений, чтобы избежать повторной обработки
    static HashSet<long> processedMessages = new HashSet<long>();

    static void Main(string[] args)
    {
        // Ваш токен сообщества
        string token = "МАКСИМ_ВСТАВЬ_СЮДА_ТОКЕН_НЕ_ЗАБУДЬ_ТОКЕН_ОН_БЕЗ_НЕГО_НЕ_БУДЕТ_РАБОТАТЬ!!!!!!!!!";

        // Инициализация API
        var api = new VkApi();
        api.Authorize(new ApiAuthParams { AccessToken = token });

        Console.WriteLine("Бот запущен!");

        while (true)
        {
            try
            {
                // Получение последних сообщений
                var messages = api.Messages.GetConversations(new GetConversationsParams
                {
                    Count = 20
                });

                foreach (var item in messages.Items)
                {
                    var message = item.LastMessage;

                    // Проверяем, что сообщение существует и не обработано ранее
                    if (message == null || message.Date == null || processedMessages.Contains(message.Id.Value))
                        continue;

                    // Добавляем сообщение в обработанные
                    processedMessages.Add(message.Id.Value);

                    // Фильтрация: исключаем беседы (где PeerId > 2000000000)
                    if (message.PeerId > 2000000000)
                        continue;

                    Console.WriteLine($"Получено сообщение от {message.PeerId}: {message.Text}");

                    // Логика обработки команды /start
                    if (message.Text == "/start")
                    {
                        // Создание клавиатуры
                        var keyboard = new MessageKeyboard
                        {
                            Buttons = new[]
                            {
                                new[]
                                {
                                    new MessageKeyboardButton
                                    {
                                        Action = new MessageKeyboardButtonAction
                                        {
                                            Label = "Кнопка 1",
                                            Type = VkNet.Enums.SafetyEnums.KeyboardButtonActionType.Text
                                        },
                                        Color = VkNet.Enums.SafetyEnums.KeyboardButtonColor.Primary
                                    },
                                    new MessageKeyboardButton
                                    {
                                        Action = new MessageKeyboardButtonAction
                                        {
                                            Label = "Кнопка 2",
                                            Type = VkNet.Enums.SafetyEnums.KeyboardButtonActionType.Text
                                        },
                                        Color = VkNet.Enums.SafetyEnums.KeyboardButtonColor.Negative
                                    },
                                     new MessageKeyboardButton
                                    {
                                        Action = new MessageKeyboardButtonAction
                                        {
                                            Label = "Кнопка 3",
                                            Type = VkNet.Enums.SafetyEnums.KeyboardButtonActionType.Text
                                        },
                                        Color = VkNet.Enums.SafetyEnums.KeyboardButtonColor.Negative
                                    }
                                }
                            },
                            OneTime = false
                        };

                        // Отправка клавиатуры
                        api.Messages.Send(new MessagesSendParams
                        {
                            RandomId = new Random().Next(),
                            PeerId = message.PeerId.Value,
                            Message = "Выберите одну из кнопок:",
                            Keyboard = keyboard
                        });

                        Console.WriteLine($"Клавиатура отправлена пользователю {message.PeerId}");
                    }
                    else if (message.Text == "Кнопка 1")
                    {
                        // Ответ на "Кнопка 1"
                        api.Messages.Send(new MessagesSendParams
                        {
                            RandomId = new Random().Next(),
                            PeerId = message.PeerId.Value,
                            Message = "Привет! Как твои дела?"
                        });

                        Console.WriteLine($"Ответ на 'Кнопка 1' отправлен пользователю {message.PeerId}");
                    }
                    else if (message.Text == "Кнопка 2")
                    {
                        // Ответ на "Кнопка 2"
                        api.Messages.Send(new MessagesSendParams
                        {
                            RandomId = new Random().Next(),
                            PeerId = message.PeerId.Value,
                            Message = "Пока! Увидимся."
                        });

                        Console.WriteLine($"Ответ на 'Кнопка 2' отправлен пользователю {message.PeerId}");
                    }
                    else
                    {
                        Console.WriteLine($"Сообщение от {message.PeerId} проигнорировано: {message.Text}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            // Задержка между запросами
            System.Threading.Thread.Sleep(1000);
        }
    }
}
