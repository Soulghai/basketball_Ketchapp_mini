// Menu
// Завершение игры

public struct OnGameOver
{
}

public struct OnStartGame
{
}

public struct OnShowMenu
{
}

public struct OnHideMenu
{
}

// Игрок заработал очки
public struct OnPointsAdd
{
    public int PointsCount;
}

// Показать индикатор очков (+ Анимация)
public struct OnPointsShow
{
}

// Сросить идикатор очков
public struct OnPointsReset
{
}

//----------
// ADS
//----------
// Дать награду игроку
public struct OnGiveReward
{
    public bool IsAvailable;
}

// Rewarded реклама готова к показу
public struct OnRewardedAvailable
{
    public bool IsAvailable;
}

// Запрос на показ рекламы 
public struct OnShowVideoAds
{
    public bool IsAvailable;
}

// Запрос на показ видео рекламы
public struct OnShowRewarded
{
    public bool IsAvailable;
}

// Можно дарить подарок
public struct OnGiftAvailable
{
    public bool IsAvailable;
}

//----------
// NOTIFICATIONS
//----------

// Показываем нотификейшины на экране
public struct OnShowNotifications
{
}

// Получили нового персонажа
public struct OnGotNewCharacter
{
}

//----------
// BUTTONS CLICKS
//----------

// Нажали на кнопку "Оценить игру"
public struct OnBtnRateClick
{
}

// Нажали на кнопку "Поделиться игрой"
public struct OnBtnShareClick
{
}

// Нажали на кнопку "Получить подарок"
public struct OnBtnGiftClick
{
    public int CoinsCount;
    public bool IsResetTimer;
}

// Нажали на кнопку "Купить рандомный скин"
public struct OnBtnGetRandomSkinClick
{
}

// Высыпать на экран горсть монет
public struct OnGiftShowCoinsAnimation
{
    public int CoinsCount;
    public bool IsResetTimer;
}

// Показать на экран Скин, который игрок получает после нажатия на ленточку "Получить скин за 200 монет"
public struct OnGiftShowRandomSkinAnimation
{
}

// Показать на экран Скин, который игрок получает после нажатия на ленточку "Получить скин за 200 монет"
public struct OnBuySkin
{
    public int Id;
}

// Закончилась анимация Вручения подарка
public struct OnGiftCollected
{
}

// Скрыть экран подарка
public struct OnHideGiftScreen
{
    public int Type;
}