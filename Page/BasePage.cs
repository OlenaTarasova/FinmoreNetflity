using Microsoft.Playwright;

namespace FinmoreNetflity.Pages;

public class BasePage
{
    public  IPage Page;
    protected const string BaseUrl = "https://finmore.netlify.app/";

    public BasePage(IPage page)
    {
        Page = page ?? throw new ArgumentNullException(nameof(page));
    }

    public async Task NavigateAsync(string relativeUrl = "")
    {
        await Page.GotoAsync($"{BaseUrl}{relativeUrl}", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        }); 
    }

    public async Task WaitForNetworkIdleAsync()
    {
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
public async Task ClickAsync(ILocator locator, string elementName = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(elementName))
                {
                    Console.WriteLine($"[CLICK] Початок кліку на '{elementName}'");
                }
                else
                {
                    Console.WriteLine($"[CLICK] Початок кліку");
                }

                await locator.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 30000
                });
                Console.WriteLine($"[CLICK] Елемент знайдено та видимий");

                var isEnabled = await locator.IsEnabledAsync();
                Console.WriteLine($"[CLICK] Елемент активний: {isEnabled}");
                if (!isEnabled)
                {
                    Console.WriteLine($"[WARNING] Елемент не активний, але спробуємо клікнути");
                }

                await locator.ClickAsync();
                Console.WriteLine($"[CLICK] Клік виконано успішно");
                // Чекаємо завантаження
                await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                Console.WriteLine($"[CLICK] Сторінка завантажена");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"[ERROR] Таймаут очікування елемента");
                Console.WriteLine($"[ERROR] Message: {ex.Message}");
                throw new Exception($"Не вдалося знайти або клікнути на елемент '{elementName}'", ex);
            }
            catch (PlaywrightException ex)
            {
                Console.WriteLine($"[ERROR] Помилка Playwright");
                Console.WriteLine($"[ERROR] Type: {ex.GetType().Name}");
                Console.WriteLine($"[ERROR] Message: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Загальна помилка");
                Console.WriteLine($"[ERROR] Type: {ex.GetType().Name}");
                Console.WriteLine($"[ERROR] Message: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                throw;
            }
            finally
            {
                Console.WriteLine($"[CLICK] Завершення методу кліку");
            }
        }
        public async Task Fill(ILocator locator, string text)
        {
            try
            {
                Console.WriteLine("========================================");
                Console.WriteLine("[FILL] НАЧАЛО ВВОДА ТЕКСТА");
                Console.WriteLine($"Текст для ввода: '{text}'");
                Console.WriteLine($"Ожидание доступности элемента...");
                await locator.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 30000
                });
                Console.WriteLine($"Элемент найден и видим");
                var isEnabled = await locator.IsEnabledAsync();
                Console.WriteLine($"Элемент активен: {(isEnabled ? "ДА" : "НЕТ")}");
                var isEditable = await locator.IsEditableAsync();
                Console.WriteLine($"Элемент редактируется: {(isEditable ? "ДА" : "НЕТ")}");
                try
                {
                    var currentValue = await locator.InputValueAsync();
                    Console.WriteLine($"Текущий текст в поле: '{currentValue}'");
                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        Console.WriteLine($"Очистка текущего текста...");
                        await locator.ClearAsync();
                        Console.WriteLine($"Текст очищен");
                    }
                }
                catch
                {
                    Console.WriteLine($"Не удалось получить текущий текст (возможно поле пустое)");
                }
                Console.WriteLine($"Ввод текста: '{text}'");
                await locator.FillAsync(text);
                try
                {
                    var enteredValue = await locator.InputValueAsync();
                    Console.WriteLine($"Проверка введенного текста...");
                    Console.WriteLine($"Фактически введенный текст: '{enteredValue}'");
                    if (enteredValue == text)
                    {
                        Console.WriteLine($"Текст введен корректно!");
                    }
                    else
                    {
                        Console.WriteLine($"ВНИМАНИЕ: Введенный текст не совпадает с ожидаемым!");
                        Console.WriteLine($"   Ожидалось: '{text}'");
                        Console.WriteLine($"   Получено: '{enteredValue}'");
                    }
                }
                catch
                {
                    Console.WriteLine($"Не удалось проверить введенный текст");
                }
                Console.WriteLine($"[FILL] ВВОД ЗАВЕРШЕН УСПЕШНО");
                Console.WriteLine("========================================\n");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine($"[FILL ERROR] ТАЙМАУТ!");
                Console.WriteLine($"Время ожидания истекло");
                Console.WriteLine($"Возможная причина: элемент не найден на странице");
                Console.WriteLine($"Детали: {ex.Message}");
                Console.WriteLine("========================================\n");
                throw;
            }
            catch (PlaywrightException ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine($"[FILL ERROR] ОШИБКА PLAYWRIGHT!");
                Console.WriteLine($"Тип ошибки: {ex.GetType().Name}");
                Console.WriteLine($"Сообщение: {ex.Message}");
                if (ex.Message.Contains("not visible"))
                {
                    Console.WriteLine($"Элемент не видим на странице");
                }
                else if (ex.Message.Contains("detached"))
                {
                    Console.WriteLine($"Элемент был отключен от DOM");
                }
                else if (ex.Message.Contains("intercepted"))
                {
                    Console.WriteLine($"Клик был перехвачен другим элементом");
                }
                Console.WriteLine("========================================\n");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("========================================");
                Console.WriteLine($"[FILL ERROR] НЕИЗВЕСТНАЯ ОШИБКА!");
                Console.WriteLine($"Что-то пошло не так");
                Console.WriteLine($"Тип ошибки: {ex.GetType().Name}");
                Console.WriteLine($"Сообщение: {ex.Message}");
                Console.WriteLine($"Stack trace:");
                Console.WriteLine($"{ex.StackTrace}");
                Console.WriteLine("========================================\n");
                throw;
            }
        }
    }

    