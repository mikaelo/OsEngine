﻿/*
 * Your rights to use code governed by this license http://o-s-a.net/doc/license_simple_engine.pdf
 *Ваши права на использования кода регулируются данной лицензией http://o-s-a.net/doc/license_simple_engine.pdf
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OsEngine.Entity;
using OsEngine.Indicators;

namespace OsEngine.Charts.CandleChart.Indicators
{
    public class Ac : IIndicator
    {

        /// <summary>
        /// constructor with parameters.Indicator will be saved
        /// конструктор с параметрами. Индикатор будет сохраняться
        /// </summary>
        /// <param name="uniqName">unique name/уникальное имя</param>
        /// <param name="canDelete">whether user can remove indicator from chart manually/можно ли пользователю удалить индикатор с графика вручную</param>
        public Ac(string uniqName, bool canDelete)
        {
            Name = uniqName;
            LengthLong = 34;
            LengthShort = 5;
            TypeIndicator = IndicatorChartPaintType.Column;
            TypeCalculationAverage = MovingAverageTypeCalculation.Simple;
            PaintOn = true;
            CanDelete = canDelete;
            ColorUp = Color.DeepSkyBlue;
            ColorDown = Color.DarkRed;
            Load();
        }

        /// <summary>
        /// constructor without parameters.Indicator will not saved/конструктор без параметров. Индикатор не будет сохраняться
        /// used ONLY to create composite indicators/используется ТОЛЬКО для создания составных индикаторов
        /// Don't use it from robot creation layer!не используйте его из слоя создания роботов!
        /// </summary>
        /// <param name="canDelete">whether user can remove indicator from chart manually/можно ли пользователю удалить индикатор с графика вручную</param>
        public Ac(bool canDelete)
        {
            Name = Guid.NewGuid().ToString();

            LengthLong = 34;
            LengthShort = 5;
            TypeIndicator = IndicatorChartPaintType.Column;
            TypeCalculationAverage = MovingAverageTypeCalculation.Simple;
            PaintOn = true;
            CanDelete = canDelete;
        }

        /// <summary>
        /// Is it possible to remove indicator from chart. This is necessary so that robots can't be deleted./можно ли удалить индикатор с графика. Это нужно для того чтобы у роботов нельзя было удалить 
        /// indicators he needs in trading/индикаторы которые ему нужны в торговле
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// all indicator values/все значения индикатора
        /// </summary>
        List<List<decimal>> IIndicator.ValuesToChart
        {
            get
            {
                List<List<decimal>> list = new List<List<decimal>>();
                list.Add(Values);
                return list;
            }
        }

        /// <summary>
        /// indicator colors/цвета для индикатора
        /// </summary>
        List<Color> IIndicator.Colors
        {
            get
            {
                List<Color> colors = new List<Color>();
                colors.Add(ColorUp);
                colors.Add(ColorDown);
                return colors;
            }

        }

        /// <summary>
        /// indicator drawing type
        /// тип прорисовки индикатора
        /// </summary>
        public IndicatorChartPaintType TypeIndicator { get; set; }

        /// <summary>
        /// type of moving average for indicator calculation
        /// тип скользящей средней для рассчёта индикатора
        /// </summary>
        public MovingAverageTypeCalculation TypeCalculationAverage;

        /// <summary>
        /// name of data series on which indicator will be drawn
        /// имя серии данных на которой будет прорисован индикатор
        /// </summary>
        public string NameSeries { get; set; }

        /// <summary>
        /// name of the data area where the indicator will be drawn
        /// имя области данных на которой будет прорисовываться индикатор
        /// </summary>
        public string NameArea { get; set; }

        /// <summary>
        /// Ac
        /// </summary>
        public List<decimal> Values
        { get; set; }

        /// <summary>
        /// unique indicator name
        /// уникальное имя индикатора
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// not used
        /// не используется
        /// </summary>
        public Color ColorDown
        { get; set; }

        /// <summary>
        /// not used
        /// не используется
        /// </summary>
        public Color ColorUp
        { get; set; }

        /// <summary>
        /// MA calculation period
        /// длинна периода для рассчёта машки
        /// </summary>
        public int LengthLong;

        /// <summary>
        /// MA calculation period
        /// длинна периода для рассчёта машки
        /// </summary>
        public int LengthShort;

        /// <summary>
        /// is indicator tracing enabled
        /// включена ли прорисовка индикатора
        /// </summary>
        public bool PaintOn { get; set; }

        /// <summary>
        /// save settings to file
        /// сохранить настройки в файл
        /// </summary>
        public void Save()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    return;
                }

                using (StreamWriter writer = new StreamWriter(@"Engine\" + Name + @".txt", false))
                {
                    writer.WriteLine(ColorUp.ToArgb());
                    writer.WriteLine(ColorDown.ToArgb());
                    writer.WriteLine(LengthLong);
                    writer.WriteLine(LengthShort);
                    writer.WriteLine(PaintOn);
                    writer.WriteLine(TypeCalculationAverage);
                    writer.Close();
                }
            }
            catch (Exception)
            {
                // send to log
                // отправить в лог
            }
        }

        /// <summary>
        /// load settings from file
        /// загрузить настройки из файла
        /// </summary>
        public void Load()
        {
            if (!File.Exists(@"Engine\" + Name + @".txt"))
            {
                return;
            }
            try
            {
                using (StreamReader reader = new StreamReader(@"Engine\" + Name + @".txt"))
                {
                    ColorUp = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                    ColorDown = Color.FromArgb(Convert.ToInt32(reader.ReadLine()));
                    LengthLong = Convert.ToInt32(reader.ReadLine());
                    LengthShort = Convert.ToInt32(reader.ReadLine());
                    PaintOn = Convert.ToBoolean(reader.ReadLine());
                    Enum.TryParse(reader.ReadLine(), true, out TypeCalculationAverage);
                    reader.ReadLine();

                    reader.Close();
                }
            }
            catch (Exception)
            {
                // send to log
                // отправить в лог
            }
        }

        /// <summary>
        /// delete file with settings
        /// удалить файл с настройками
        /// </summary>
        public void Delete()
        {
            if (File.Exists(@"Engine\" + Name + @".txt"))
            {
                File.Delete(@"Engine\" + Name + @".txt");
            }
        }

        /// <summary>
        /// show window with settings
        /// показать окно с настройками
        /// </summary>
        public void ShowDialog()
        {
            AcUi ui = new AcUi(this);
            ui.ShowDialog();

            if (ui.IsChange && _myCandles != null)
            {
                Reload();
            }
        }

        /// <summary>
        /// reload indicator
        /// перезагрузить индикатор
        /// </summary>
        public void Reload()
        {
            if (_myCandles == null)
            {
                return;
            }
            ProcessAll(_myCandles);

            if (NeedToReloadEvent != null)
            {
                NeedToReloadEvent(this);
            }
        }

        /// <summary>
        /// candles to calculate indicator
        /// свечи для рассчёта индикатора
        /// </summary>
        private List<Candle> _myCandles;

        /// <summary>
        /// calculate indicator
        /// рассчитать индикатор
        /// </summary>
        /// <param name="candles">candles/свечи</param>
        public void Process(List<Candle> candles)
        {
            _myCandles = candles;

            if (_ao == null)
            {
                _ao = new AwesomeOscillator(false);
                _ao.LengthLong = LengthLong;
                _ao.LengthShort = LengthShort;

                _movingAverage = new MovingAverage(false);
                _movingAverage.Length = LengthShort;
            }

            _ao.Process(candles);
            _movingAverage.Process(_ao.Values);

            if (Values != null &&
                Values.Count + 1 == candles.Count)
            {
                ProcessOne(candles);
            }
            else if (Values != null &&
                     Values.Count == candles.Count)
            {
                ProcessLast(candles);
            }
            else
            {
                ProcessAll(candles);
            }
        }

        /// <summary>
        /// indicator needs to be redrawn
        /// индикатор нужно перерисовать
        /// </summary>
        public event Action<IIndicator> NeedToReloadEvent;

        /// <summary>
        /// load only last candle
        /// прогрузить только последнюю свечку
        /// </summary>
        private void ProcessOne(List<Candle> candles)
        {
            if (candles == null)
            {
                return;
            }

            if (Values == null)
            {
                Values = new List<decimal>();
                Values.Add(GetValue(candles.Count - 1));
            }
            else
            {
                Values.Add(GetValue(candles.Count - 1));
            }
        }

        /// <summary>
        /// to upload from the beginning
        /// прогрузить с самого начала
        /// </summary>
        private void ProcessAll(List<Candle> candles)
        {
            if (candles == null)
            {
                return;
            }
            Values = new List<decimal>();

            _ao = new AwesomeOscillator(false);
            _ao.LengthLong = LengthLong;
            _ao.LengthShort = LengthShort;

            _ao.Process(candles);

            _movingAverage = new MovingAverage(false);
            _movingAverage.Length = LengthShort;
            _movingAverage.Process(_ao.Values);

            for (int i = 0; i < candles.Count; i++)
            {
                Values.Add(GetValue( i));
            }
        }

        /// <summary>
        /// overload last value
        /// перегрузить последнее значение
        /// </summary>
        private void ProcessLast(List<Candle> candles)
        {
            if (candles == null)
            {
                return;
            }

            _ao.Process(candles);

            _movingAverage.Process(_ao.Values);

            Values[Values.Count - 1] = GetValue(candles.Count - 1);
        }

        /// <summary>
        /// take the indicator value by index/взять значение индикатора по индексу
        /// </summary>
        /// <param name="candles">candles/свечи</param>
        /// <param name="index">index/индекс</param>
        /// <returns>index value/значение индикатора по индексу</returns>
        private decimal GetValue(int index)
        {
            if (
                index < LengthShort || index < LengthLong ||
               _ao.Values[index] == 0)
            {
                return 0;
            }

            decimal value = 0;

            value = _ao.Values[index] - _movingAverage.Values[index];


            return value;
        }

        /// <summary>
        /// average for calculation AwesomeOscilliator
        /// средняя для рассчёта AwesomeOscilliator
        /// </summary>
        private AwesomeOscillator _ao;

        /// <summary>
        /// average smoothing indicator AwesomeOscilliator
        /// средняя для сглаживания индикатора AwesomeOscilliator
        /// </summary>
        private MovingAverage _movingAverage;

        /// <summary>
        /// delete data
        /// удалить данные
        /// </summary>
        public void Clear()
        {
            if (Values != null)
            {
                Values.Clear();
            }
            _myCandles = null;
        }
    }
}