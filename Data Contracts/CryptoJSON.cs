public class BTC
{
    public BTC_USDT[] BTC_USDT { get; set; }
}

public class BTC_USDT
{
    public int trade_id { get; set; }
    public int date { get; set; }
    public string type { get; set; }
    public string quantity { get; set; }
    public string price { get; set; }
    public string amount { get; set; }
}

public class ETH
{
    public ETH_USDT[] ETH_USDT { get; set; }
}

public class ETH_USDT
{
    public int trade_id { get; set; }
    public int date { get; set; }
    public string type { get; set; }
    public string quantity { get; set; }
    public string price { get; set; }
    public string amount { get; set; }
}

public class USDT
{
    public USDT_USD[] USDT_USD { get; set; }
}

public class USDT_USD
{
    public int trade_id { get; set; }
    public int date { get; set; }
    public string type { get; set; }
    public string quantity { get; set; }
    public string price { get; set; }
    public string amount { get; set; }
}

public class CryptoJson
{
    public Crypto[] crypto { get; set; }
}

public class Crypto
{
    public int trade_id { get; set; }
    public int date { get; set; }
    public string type { get; set; }
    public string quantity { get; set; }
    public string price { get; set; }
    public string amount { get; set; }
}