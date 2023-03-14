using CasosSospechososMI.Domain.Family.Enums;
using CasosSospechososMI.Domain.Samples;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.UseCases.Account;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CasosSospechososMI.UI.Records.ViewModels
{
    public class RecordDetailViewModel : BaseViewModel
    {
        GetHomeUserData _getHomeData;
        public RecordDetailViewModel(GetHomeUserData getHomeUserData, IRoutingService routingService) : base(routingService)
        {
            _getHomeData = getHomeUserData;
            Sample = new SampleModel();
        }
        #region Properites
        string _mensaje;
        public string MensajeWhp
        {
            get => _mensaje;
            set
            {
                SetProperty(ref _mensaje, value);
            }
        }
        StackLayout _general;
        public StackLayout General
        {
            get => _general;
            set
            {
                SetProperty(ref _general, value);
            }
        }
        SampleModel _sample;
        public SampleModel Sample
        {
            get => _sample;
            set
            {
                SetProperty(ref _sample, value);
            }
        }
        #endregion
        internal void OnAppearing()
        {
            BuildSampleRecords();
            var user =  _getHomeData.Invoke();
            MensajeWhp = (user != null && !string.IsNullOrEmpty(user.Ovitrampa)) ?
                $"Hola, necesito cambiar la ovitrampa {user.Ovitrampa}." : $"Hola, tengo problemas con mi ovitrampa.";
        }

        private void BuildSampleRecords()
        {
            if (Sample.HasRes1)
            {
                BuildItemSampleRecord(Sample.Res1);
            }if (Sample.HasRes2)
            {
                BuildItemSampleRecord(Sample.Res2);
            }if (Sample.HasRes3)
            {
                BuildItemSampleRecord(Sample.Res3);
            }if (Sample.HasRes4)
            {
                BuildItemSampleRecord(Sample.Res4);
            }
            //if (Sample.HasComment)
            //{
            //    var item = new SampleUnit() { Pregunta = "Comentarios", Descripcion = Sample.Comentario };
            //    BuildItemSampleRecord(item);
            //}
        }


        private void BuildItemSampleRecord(SampleUnit item)
        {
            switch (item.Tipo)
            {
                case QuestionEnum.Check:
                    BuildCheckAnswer(item);
                    //BuildCheckQuestionView(item);
                    break;
                default:
                    BuildTextAnswer(item);
                    break;

            }
        }

        private void BuildTextAnswer(SampleUnit item)
        {
            Grid grid = new Grid
            {
                BackgroundColor = Color.Transparent,
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(80) },
                    new RowDefinition()
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };
            grid.Children.Add(new BoxView
            {
                Color = Color.Transparent
            });
            grid.Children.Add(new Label
            {
                Text = item.Pregunta,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            });
            grid.Children.Add(new BoxView
            {
                Color = Color.Transparent
            }, 1, 0);


            Label editor = new Label
            {
                FontSize = 16,
                Margin = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Text = item.Descripcion
            };

            grid.Children.Add(editor, 1, 0);

            BoxView boxView = new BoxView
            {
                Margin = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Color = (Color)Application.Current.Resources["Comment"],
                HeightRequest = 1
            };
            Grid.SetRow(boxView, 1);
            General.Children.Add(grid);
            General.Children.Add(boxView);
        }


        private void BuildCheckAnswer(SampleUnit item)
        {
            Grid grid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition(),
                    new RowDefinition{Height = new GridLength(10) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };
            grid.Children.Add(new BoxView
            {
                Color = Color.Transparent
            });
            grid.Children.Add(new Label
            {
                Text = item.Pregunta,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            });
            grid.Children.Add(new BoxView
            {
                Color = Color.Transparent
            }, 1, 0);

            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
            };
            CheckBox checkYes = new CheckBox() { IsEnabled = false};
            checkYes.Color = (Color)Application.Current.Resources["Primary"];
            
            CheckBox checkNo = new CheckBox() { IsEnabled = false };
            
            checkNo.Color = (Color)Application.Current.Resources["Primary"];
            if (item.Descripcion == "S")
            {
                checkYes.IsChecked = true;
            }
            else
            {
                checkNo.IsChecked = true;
            }
            stack.Children.Add(new Label
            {
                Text = "Si",
                VerticalTextAlignment = TextAlignment.Center
            });
            stack.Children.Add(checkYes);
            stack.Children.Add(new Label
            {
                Text = "No",
                VerticalTextAlignment = TextAlignment.Center
            });
            stack.Children.Add(checkNo);

            grid.Children.Add(stack, 1, 0);
            BoxView boxView = new BoxView
            {
                Margin = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Color = (Color)Application.Current.Resources["Comment"],
                HeightRequest = 1
            };
            Grid.SetRow(boxView, 1);
            General.Children.Add(grid);
            General.Children.Add(boxView);
        }
    }
}
