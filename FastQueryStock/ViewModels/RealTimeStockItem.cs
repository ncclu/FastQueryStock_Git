﻿using FastQueryStock.Service;
using StockSDK;
using StockSDK.Google;
using StockSDK.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FastQueryStock.ViewModels
{
    public class RealTimeStockItem : BaseViewModel
    {
        #region Fields
        private string _currentPrice;
        private string _highPrice;
        private string _lowPrice;
        private string _changePrice;
        private string _lastTime;
        private string _changePercentage;
        private double _volumes;
        private string _currentTimeVolumes;
        private string _buyQuantityList;
        private string _sellPriceList;
        private string _buyPriceList;
        private string _sellQuantityList;
        #endregion

        #region UI Properties (Read from RESTful api)
        public string Name { get; set; }

        public string Id { get; set; }

        /// <summary>
        /// 漲跌值
        /// </summary>
        public string ChangePrice
        {
            get { return _changePrice; }
            set
            {
                _changePrice = value;
                NotifyPropertyChanged("ChangePrice");
                NotifyPropertyChanged("ChangePriceStatus");
                NotifyPropertyChanged("ValueColor");
            }
        }

        /// <summary>
        /// 成交價
        /// </summary>
        public string CurrentPrice
        {
            get { return _currentPrice; }
            set
            {
                _currentPrice = value;
                NotifyPropertyChanged("CurrentPrice");
                NotifyPropertyChanged("ChangePriceStatus");
                NotifyPropertyChanged("CurrentPriceValueColor");
                NotifyPropertyChanged("CurrentPriceValueBackgroundColor");

            }
        }

        /// <summary>
        /// 漲跌幅
        /// </summary>
        public string ChangePercentage
        {
            get
            {
                if (!string.IsNullOrEmpty(_changePercentage))
                {
                    return _changePercentage.Replace("-", string.Empty).Replace("%", string.Empty) + "%";
                }
                return string.Empty;
            }
            set
            {
                _changePercentage = value;
                NotifyPropertyChanged("ChangePercentage");
            }
        }


        public string HighestPrice
        {
            get { return _highPrice; }
            set
            {
                _highPrice = value;
                NotifyPropertyChanged("HighestPrice");
                NotifyPropertyChanged("HighestPriceValueColor");
                NotifyPropertyChanged("HighestPriceValueBackgroundColor");
            }
        }

        public string LowestPrice
        {
            get
            { return _lowPrice; }
            set
            {
                _lowPrice = value;
                NotifyPropertyChanged("LowestPrice");
                NotifyPropertyChanged("LowestPriceValueColor");
                NotifyPropertyChanged("LowestPriceValueBackgroundColor");
            }
        }
        public string OpenPrice { get; set; }

        /// <summary>
        /// 最後時間
        /// </summary>
        public string LatestTime
        {
            get { return _lastTime; }
            set
            {
                _lastTime = value;
                NotifyPropertyChanged("LatestTime");
            }
        }

        /// <summary>
        /// 成交張數
        /// </summary>
        public double Volumes
        {
            get { return _volumes; }
            set
            {
                _volumes = value;
                NotifyPropertyChanged("Volumes");
            }
        }
        /// <summary>
        /// 當筆成交量
        /// </summary>
        public string CurrentTimeVolumes
        {
            get { return _currentTimeVolumes; }
            set
            {
                _currentTimeVolumes = value;
                NotifyPropertyChanged("CurrentTimeVolumes");

            }
        }

        /// <summary>
        /// 揭示買價(從高到低，以_分隔資料), e.g : 27.40_27.35_27.30_27.25_27.20_
        /// </summary>
        public string BuyPriceList
        {
            get { return _buyPriceList; }
            set
            {
                _buyPriceList = value;
                NotifyPropertyChanged("BuyPriceList");
            }
        }

        /// <summary>
        /// 揭示買量(配合b，以_分隔資料) e.g. : 4_23_79_20_48_
        /// </summary>
        public string BuyQuantityList
        {
            get { return _buyQuantityList; }
            set
            {
                _buyQuantityList = value;
                NotifyPropertyChanged("BuyQuantityList");
            }
        }

        /// <summary>
        /// 揭示賣價(從低到高，以_分隔資料) e.g : 27.45_27.50_27.55_27.60_27.65_
        /// </summary>
        public string SellPriceList
        {
            get { return _sellPriceList; }
            set
            {
                _sellPriceList = value;
                NotifyPropertyChanged("SellPriceList");
            }
        }

        /// <summary>
        /// 揭示賣量(配合a，以_分隔資料) e.g. : 12_96_61_120_62_
        /// </summary>
        public string SellQuantityList
        {
            get { return _sellQuantityList; }
            set
            {
                _sellQuantityList = value;
                NotifyPropertyChanged("SellQuantityList");
            }
        }

        /// <summary>
        /// 漲停價
        /// </summary>
        public string LimitUp { get; set; }
        /// <summary>
        /// 跌停價
        /// </summary>
        public string LimitDown { get; set; }

        /// <summary>
        /// 昨收價格
        /// </summary>
        public string YesterdayPrice { get; set; }


        /// <summary>
        /// Display the change status of stock
        /// </summary>
        public string ChangePriceStatus
        {
            get
            {
                if (ChangePercentage != null && ChangePrice != null)
                    return ChangePrice + " (" + ChangePercentage + ")";
                else
                    return string.Empty;
            }
        }

        public Brush ValueColor
        {
            get
            {
                if (ChangePrice == null)
                    return Brushes.Black;

                if (ChangePrice.StartsWith("+"))
                    return Brushes.Red;
                else if (ChangePrice.StartsWith("-"))
                    return Brushes.Green;
                else
                    return Brushes.Black;
            }
        }

        public Brush CurrentPriceValueColor
        {
            get { return GetValueForegroundColor(CurrentPrice, YesterdayPrice); }
        }

        public Brush HighestPriceValueColor
        {
            get { return GetValueForegroundColor(HighestPrice, YesterdayPrice); }
        }

        public Brush LowestPriceValueColor
        {
            get { return GetValueForegroundColor(LowestPrice, YesterdayPrice); }
        }

        public Brush OpenPriceValueColor
        {
            get { return GetValueForegroundColor(OpenPrice, YesterdayPrice); }
        }

        public Brush HighestPriceValueBackgroundColor
        {
            get { return GetValueBackgroundColor(HighestPrice); }
        }

        public Brush LowestPriceValueBackgroundColor
        {
            get { return GetValueBackgroundColor(LowestPrice); }
        }

        public Brush OpenPriceValueBackgroundColor
        {
            get { return GetValueBackgroundColor(OpenPrice); }
        }

        public Brush CurrentPriceValueBackgroundColor
        {
            get { return GetValueBackgroundColor(CurrentPrice); }
        }



        /// <summary>
        /// 取得是否為漲跌停板的背景色
        /// </summary>
        /// <param name="compareValue">compare value</param>
        /// <returns></returns>
        private Brush GetValueBackgroundColor(string compareValue)
        {
            Brush result= null;
            if (!string.IsNullOrEmpty(compareValue) && !string.IsNullOrEmpty(LimitDown) && !string.IsNullOrEmpty(LimitUp))
            {
                decimal limitUp = Convert.ToDecimal(LimitUp);
                decimal limitDown = Convert.ToDecimal(LimitDown);
                decimal value = Convert.ToDecimal(compareValue);
                if (value >= limitUp)
                    result = Brushes.Red;
                else if (value <= limitDown)
                    result = Brushes.Green;
            }
            return result;
        }

        private Brush GetValueForegroundColor(string compareValue, string baseValue)
        {
            Brush result = Brushes.Black;
            if (!string.IsNullOrEmpty(compareValue) && !string.IsNullOrEmpty(baseValue))
            {
                // if the stock value is limit up or limit down, return the wihte color
                Brush backgroundColor = GetValueBackgroundColor(compareValue);
                if (backgroundColor == Brushes.Red || backgroundColor == Brushes.Green)
                    return Brushes.White;


                decimal valueDiff = Convert.ToDecimal(compareValue) - Convert.ToDecimal(baseValue);

                if (valueDiff > 0)
                    result = Brushes.Red;
                else if (valueDiff < 0)
                    result = Brushes.Green;
            }
            return result;
        }

        #endregion

        /// <summary>
        /// 上市上櫃代號
        /// </summary>
        public string ExchangeTypeKey { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="queryProvider">提供股票查詢的服務實體</param>
        /// <returns></returns>
        public static RealTimeStockItem ConvertFrom(RealTimeStockModel model)
        {
            double vol = -1;
            if (model.Volumes != null)
                vol = Convert.ToDouble(model.Volumes.Replace(",", string.Empty));

            return new RealTimeStockItem()
            {
                Id = model.Id,
                Name = model.Name,
                ChangePercentage = model.ChangePercentage,
                CurrentPrice = model.CurrentPrice,
                ChangePrice = model.ChangePrice,
                HighestPrice = model.HighestPrice,
                LowestPrice = model.LowestPrice,
                LatestTime = model.QuerySystemTime,
                OpenPrice = model.OpenPrice,
                Volumes = Math.Truncate(vol),
                CurrentTimeVolumes = model.CurrentTimeVolumes,
                LimitUp = model.LimitUp,
                LimitDown = model.LimitDown,
                YesterdayPrice = model.YesterdayPrice,
                BuyPriceList = model.BuyPriceList,
                BuyQuantityList = model.BuyQuantityList,
                SellPriceList = model.SellPriceList,
                SellQuantityList = model.SellQuantityList,
                ExchangeTypeKey = model.ExchangeTypeKey
            };
        }


        /// <summary>
        /// Start to auto update the stock according to the interval
        /// </summary>
        /// <param name="interval">Interval time(sec)</param>

        public void Update(RealTimeStockItem newData)
        {
            this.ChangePercentage = newData.ChangePercentage;
            this.ChangePrice = newData.ChangePrice;
            this.CurrentPrice = newData.CurrentPrice;
            this.HighestPrice = newData.HighestPrice;
            this.LowestPrice = newData.LowestPrice;
            this.Volumes = newData.Volumes;
            this.LatestTime = newData.LatestTime;
            this.CurrentTimeVolumes = newData.CurrentTimeVolumes;
            this.BuyPriceList = newData.BuyPriceList;
            this.BuyQuantityList = newData.BuyQuantityList;
            this.SellPriceList = newData.SellPriceList;
            this.SellQuantityList = newData.SellQuantityList;
        }
    }
}
