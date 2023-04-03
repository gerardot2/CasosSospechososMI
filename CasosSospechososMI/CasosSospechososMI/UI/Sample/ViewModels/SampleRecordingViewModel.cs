using Akavache;
using FFImageLoading;
using CasosSospechososMI.Abstractions;
using CasosSospechososMI.Controls;
using CasosSospechososMI.Domain.Common;
using CasosSospechososMI.Domain.Family;
using CasosSospechososMI.Domain.Family.Enums;
using CasosSospechososMI.Services.Interfaces;
using CasosSospechososMI.UI.Base.ViewModels;
using CasosSospechososMI.UI.Common.Views;
using CasosSospechososMI.UseCases.Account;
using CasosSospechososMI.UseCases.Sample;
using CasosSospechososMI.UseCases.Supervisor;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using System.Reactive.Linq;
using CasosSospechososMI.Models;
using CasosSospechososMI.UseCases.Common;
using CasosSospechososMI.Domain.Account;

namespace CasosSospechososMI.UI.Sample.ViewModels
{
    [QueryProperty("Code", "code")]
    public class SampleRecordingViewModel : BaseViewModel
    {
        #region Commands
        public Command CheckYesCommand { get; }
        public Command CameraCommmand { get; }
        public Command SubmitCommand { get; }
        #endregion
        private const string DATE_FORMAT = "yyyy'-'MM'-'dd HH':'mm:''ss";
        GetSampleForm _getSampleForm;
        GetActualLocation _getActualLocation;
        PostSampleRecord _postSampleRecord;
        PostVisitRecord _postVisitRecord;
        GetActualUser _getActualUser;
        IRoutingService _routingService;
        GetCities _getCities;
        UpdateActualUser _updateActualUser;

        GetHomeUserData _getHomeData;
        public SampleRecordingViewModel(GetSampleForm getSampleForm
            , GetActualLocation getActualLocation
            , PostSampleRecord postSampleRecord
            , IRoutingService routingService
            , GetHomeUserData getHomeUserData,
            PostVisitRecord postVisitRecord,
            GetCities getCities,
GetActualUser getActualUser,
UpdateActualUser updateActualUser) : base(routingService)
        {
            _getSampleForm = getSampleForm;
            _getActualLocation = getActualLocation;
            _postSampleRecord = postSampleRecord;
            _routingService = routingService;
            _getHomeData = getHomeUserData;
            CameraCommmand = new Command(TakePhotoAsync);
            SubmitCommand = new Command(SubmitFormAsync);
            FormItems = new ObservableCollection<FormModel>();
            FormRecord = new FormRecordModel();
            _postVisitRecord = postVisitRecord;
            _getActualUser = getActualUser;
            _updateActualUser = updateActualUser;
            _getCities = getCities;
            SelectedCity = new City();
            Cities = new ObservableCollection<City>();
        }


        #region Properties
        public bool completed { get; set; } = false;
        public bool cameraOpened { get; set; } = false;
        public bool popupOpened { get; set; } = false;
        private StreamPart Imagen { get; set; }
        private byte[] ImagenToCache { get; set; }
        private ObservableCollection<FormModel> _formItems;
        public ObservableCollection<FormModel> FormItems
        {
            get => _formItems;
            set
            {
                SetProperty(ref _formItems, value);
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
        FormRecordModel _formRecord;
        public FormRecordModel FormRecord
        {
            get => _formRecord;
            set
            {
                SetProperty(ref _formRecord, value);
            }
        }
        bool _isButtonEnabled = false;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                SetProperty(ref _isButtonEnabled, value);
            }
        }
        string _photoPath;
        public string PhotoPath
        {
            get => _photoPath;
            set
            {
                SetProperty(ref _photoPath, value);
                OnPropertyChanged("PhotoPath");
            }
        }
        bool _isImageLoading = false;
        public bool IsImageLoading
        {
            get => _isImageLoading;
            set
            {
                SetProperty(ref _isImageLoading, !string.IsNullOrEmpty(PhotoPath));
            }
        }
        string _titleHeader = "Registrar Muestra";
        public string TitleHeader
        {
            get => _titleHeader;
            set
            {
                SetProperty(ref _titleHeader, value);
            }
        }
        bool _hasPhoto;
        public bool HasPhoto
        {
            get => _hasPhoto;
            set
            {
                SetProperty(ref _hasPhoto, value);
            }
        }
        bool _hasItems = false;
        public bool HasItems
        {
            get => _hasItems;
            set
            {
                SetProperty(ref _hasItems, value);
            }
        }
        string _code;
        public string Code
        {
            get => _code;
            set
            {
                SetProperty(ref _code, value);
            }
        }
        ObservableCollection<City> _cities;
        public ObservableCollection<City> Cities
        {
            get => _cities;
            set
            {
                SetProperty(ref _cities, value);
            }
        }
        City _selectedCity;
        public City SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedCity, value);
                }
            }
        }
        #endregion 
        internal async void OnAppearing()
        {
            CancellationTokenSource = new CancellationTokenSource();
            var superv = new Domain.Account.User();

            await CheckIfAvailable(cameraOpened, IsSupervisor);
            if (!Cities.Any())
            {
                
                await SearchCities();
            }
            if (!IsConnected && FormItems.Count() == 0 )
            {
                await CheckAndLoadCached();
            }
            if (FormItems.Count() == 0 && IsConnected)
            {

                superv = _getActualUser.Invoke();
                await SearchFormItems();
            }
            
        }

        public async Task SearchCities()
        {
            if (IsBusy) return;
            IsBusy = true;
            var result = await _getCities.Invoke(CancellationTokenSource.Token);
            if (result != null
                && result.Data != null)
            {
                Cities = new ObservableCollection<City>(result.Data);
            }
            else
            {
                Cities = new ObservableCollection<City>();
            }
            IsBusy = false;
        }

        private async Task CheckAndLoadCached()
        {
            try
            {
                if (_getActualUser.Invoke().HasCachedForm)
                {
                    var resp = await BlobCache.LocalMachine.GetObject<OperationResult<List<FormModel>>>("formModel");

                    FormItems.Clear();
                    FormItems = new ObservableCollection<FormModel>(resp.Data);
                    FormRecord.FormItemsNumber = FormItems.Count();
                    CreateQuestions();
                }
                else
                {
                    await OpenResultWindow("Registro de Muestra", $"Todavía no es necesario que registre una nueva muestra.", Pr_VolverInicio);
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task CheckIfAvailable(bool flag = false, bool superv = false)
        {
            var result = _getHomeData.Invoke();
            //----- Test------
            //result.DiasParaProximaMuestra = 2;
            //----- Test------
            var keys = await BlobCache.LocalMachine.GetAllKeys();
            popupOpened = true;
            if (keys.Contains("sampleModel"))
            {
                await OpenResultWindow("Registro de Muestra", $"Tiene una muestra pendiente de envío. Conéctese a Internet.", Pr_VolverInicio);
                completed = true;
            }
            if (!keys.Contains("formModel") && !IsConnected)
            {
                await OpenResultWindow("Registro de Muestra", $"Conéctese a internet para obtener el formulario.", Pr_VolverInicio);
                
            flag = true;
            }
                var totalDias = result?.DiasParaProximaMuestra > 1 ? $"Faltan {result.DiasParaProximaMuestra} dias" : "Falta 1 dia";
            if (result == null || result.DiasParaProximaMuestra > 0 && !flag && !superv)        
            {
                    
                await OpenResultWindow("Registro de Muestra", $"Todavía no es necesario que registre una nueva muestra.\n{totalDias}");
            }
            cameraOpened = false;
            popupOpened = false;
        }

        

        private async void SubmitFormAsync(object obj)
        {
            try
            {
                FormRecord.Date = DateTime.Now.ToString(DATE_FORMAT);
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                var isGpsEnable = Xamarin.Forms.DependencyService.Get<IGpsDependencyService>().IsGpsEnable();
                if (!isGpsEnable && status != PermissionStatus.Granted)
                {
                    Xamarin.Forms.DependencyService.Get<IGpsDependencyService>().OpenSettings();
                    return;
                }
                IsBusy = true;
                var location = await _getActualLocation.Invoke();
                IsBusy = false;
                if (location != null && location.IsComplete)
                {
                    FormRecord.Latitude = location.Latitude;
                    FormRecord.Longitude = location.Longitude;
                }
                if (FormRecord.FormFulfilled)
                {
                    IsBusy = true;
                    FormRecord.LocalidadId = (SelectedCity != null) ? SelectedCity.Id.ToString() : "";
                    if (!IsConnected)
                    {
                        try
                        {
                            var update = _getActualUser.Invoke();
                                                        
                            var cachedObject = (FormRecord, ImagenToCache);
                            await BlobCache.LocalMachine.InsertObject<(FormRecordModel, byte[])>("sampleModel", cachedObject,null);
                            if (update != null) { update.HasCachedRecord = true; _updateActualUser.Invoke(update); };
                            

                            await ActionsPostSubmit(true);
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                    else
                    {

                        var response = await _postSampleRecord.Invoke(CancellationTokenSource.Token, FormRecord, Imagen);
                        if (response != null && int.Parse(response.Codigo) == 0)
                        {
                            // Registro exitoso

                            await ActionsPostSubmit(false);
                            // Actualizar vista

                        }
                        else
                        {
                            popupOpened = true;
                            await OpenResultWindow("Error en envío", "Hubo un error al registrar la muestra, intente nuevamente.");
                            popupOpened = false;
                            //FormRecord.Image = null;
                            //PhotoPath = null;
                            //HasPhoto = false;
                            // Registro fallido

                        }
                    }
                    IsBusy = false;
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private async Task ActionsPostSubmit(bool cache)
        {
            DeleteAllImages();
            completed = true;
            PhotoPath = null;
            HasPhoto = false;
            FormItems = new ObservableCollection<FormModel>();
            FormRecord = new FormRecordModel();
            if (cache)
            {
                await Shell.Current.GoToAsync("..");
                await OpenResultWindow("Muestra almacenada", "Cuando vuelva a tener conexión, la muestra se enviará automaticamente.", Pr_VolverInicio);
            }
            else
            {
                await OpenResultWindow("Muestra enviada", "Podrá ver los registros y estados de sus muestras en Ver Muestras", Pr_VolverInicio);

            }
            
        }

        async void TakePhotoAsync()
        {
            try
            {
                cameraOpened = true;
                popupOpened = true;
                var photo = await Device.InvokeOnMainThreadAsync(async () => await MediaPicker.CapturePhotoAsync());
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                return;
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
                return;
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            
            // canceled
            if (photo == null)
            {
                return;
            }
            // save the file into local storage

            DeleteAllImages();
            photo.FileName = $"muestra-{DateTime.Now.ToString("ddMMyyyy'-'HHmmss")}.jpg";
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
            
            HasPhoto = !string.IsNullOrEmpty(PhotoPath);
            try
            {

                var stream = await ImageService.Instance.LoadFile(PhotoPath)
                            .DownSample(width: 1024)
                            .AsPNGStreamAsync();
                FormRecord.Image = PhotoPath;
                ImagenToCache = !IsConnected ? stream.ToByteArray() : null;
                Imagen = new StreamPart(stream, photo.FileName
                    , contentType: MediaType.ImageDefault
                    );
                IsButtonEnabled = FormRecord.FormFulfilledNoLocation;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private async Task SearchFormItems()
        {
            if (IsBusy) return;
            IsBusy = true;
            var items = await _getSampleForm.Invoke(CancellationTokenSource.Token);
            if (items != null && int.Parse(items.Codigo) == 0 && items.Data.Any())
            {
                FormItems = new ObservableCollection<FormModel>(items.Data);
                FormRecord.FormItemsNumber = FormItems.Count();
            }
            if (FormItems != null)
            {
                CreateQuestions();
            }
            IsBusy = false;
        }

        private void CreateQuestions()
        {
            HasItems = true;
            foreach (var item in FormItems.OrderBy(c => c.NumeroPregunta))
            {
                switch (item.TipoPregunta)
                {
                    case QuestionEnum.Check:
                        BuildCheckQuestion(item);
                        //BuildCheckQuestionView(item);
                        break;
                    case QuestionEnum.Select:
                        BuildSelectQuestion(item);
                        //BuildCheckQuestionView(item);
                        break;
                    case QuestionEnum.Text:
                        BuildTextQuestion(item);
                        //BuildCheckQuestionView(item);
                        break;
                    default:
                        break;

                }
            }
            BuildResultQuestion();
        }

        private void DeleteAllImages() {
            var cacheDir = FileSystem.CacheDirectory; 
            string[] allFiles = Directory.GetFiles(cacheDir); 
            foreach (string oneFile in allFiles) 
            { 
                if (File.Exists(oneFile)) File.Delete(oneFile); 
            }
        }
         
        private void BuildTextQuestion(FormModel item)
        {
            Grid grid = new Grid
            {
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
                Color = Color.White
            });
            grid.Children.Add(new Label
            {
                Text = item.Descripcion,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            });
            grid.Children.Add(new BoxView
            {
                Color = Color.White
            }, 1, 0);

            
            RoundedEditor editor = new RoundedEditor
            {
                FontSize = 16,
                Margin = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            editor.Completed += (sender, e) =>
            {
                SaveItemAnswer(item.Id.ToString(),item.NumeroPregunta,((Editor)sender).Text);
            };
            
            grid.Children.Add(editor, 1, 0);
            
            BoxView boxView = new BoxView
            {
                Margin = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Color = Color.Gray,
                HeightRequest = 1
            };
            Grid.SetRow(boxView, 1);
            General.Children.Add(grid);
            General.Children.Add(boxView);
        }

        private void BuildSelectQuestion(FormModel item)
        {
            Grid grid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition(),
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
                Color = Color.White
            });
            grid.Children.Add(new Label
            {
                Text = item.Descripcion,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            });
            grid.Children.Add(new BoxView
            {
                Color = Color.White
            }, 1, 0);

            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
            };
            CustomPicker picker = new CustomPicker
            {
                FontSize = 16,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Title = "Seleccione respuesta",
                TextColor = Color.Gray,
                ItemsSource = item.Opciones,
                SelectedIndex = -1,
                
            };
            picker.SelectedIndexChanged += (sender, e) =>
            {
                SaveItemAnswer(item.Id.ToString(),item.NumeroPregunta, picker.SelectedItem.ToString());
            };
            grid.Children.Add(picker,1,0);

            BoxView boxView = new BoxView
            {
                Margin = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Color = Color.Gray,
                HeightRequest = 1
            };
            Grid.SetRow(boxView, 1);
            General.Children.Add(grid);
            General.Children.Add(boxView);




        }

        private void BuildCheckQuestion(FormModel item)
        {
            Grid grid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition()
                    //,
                    //new RowDefinition()
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };
            grid.Children.Add(new BoxView
            {
                VerticalOptions = LayoutOptions.Center,
                Color = Color.White
            });
            grid.Children.Add(new Label
            {
                Text = item.Descripcion,
                FontSize = 16,
                Margin = new Thickness(0, 10),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            });
            grid.Children.Add(new BoxView
            {
                Color = Color.White
            }, 1, 0);

            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center,
            };
            CheckBox checkYes = new CheckBox();
            checkYes.Color = (Color)Application.Current.Resources["Primary"];
            checkYes.CheckedChanged += (sender, e) =>
            {
                // Perform required operation after examining e.Value

                var list = stack.Children;
                CheckBox checkNo = (CheckBox)list[3];
                var item0 = item.NumeroPregunta;
                if (e.Value.ToString() == "True") { checkNo.IsChecked = false;
                    SaveItemAnswer(item.Id.ToString(),item0, "S");
                }
                else
                {
                    SaveItemAnswer(item.Id.ToString(),item0, "");
                };
            };
            CheckBox checkNo = new CheckBox();
            checkNo.CheckedChanged += (sender, e) =>
            {
                // Perform required operation after examining e.Value
                var list = stack.Children;
                CheckBox checkYes = (CheckBox)list[1];
                var item0 = item.NumeroPregunta;
                if (e.Value.ToString() == "True")
                {
                    checkYes.IsChecked = false;
                    SaveItemAnswer(item.Id.ToString(),item0, "N");
                }
                else
                {
                    SaveItemAnswer(item.Id.ToString(),item0, "");
                };
            };
            checkNo.Color = (Color)Application.Current.Resources["Primary"];
            stack.Children.Add(new Label
            {
                Text = "Si",
                FontSize = 16,
                VerticalTextAlignment = TextAlignment.Center
            });
            stack.Children.Add(checkYes);
            stack.Children.Add(new Label
            {
                Text = "No",
                FontSize = 16,
                VerticalTextAlignment = TextAlignment.Center
            });
            stack.Children.Add(checkNo);
            
            grid.Children.Add(stack,1,0);
            BoxView boxView = new BoxView
            {
                Margin = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Color = Color.Gray,
                HeightRequest = 1
            };
            Grid.SetRow(boxView, 1);
            General.Children.Add(grid);
            General.Children.Add(boxView);

            
        }
        private void BuildResultQuestion()
        {
            Grid grid = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition()
                    //,
                    //new RowDefinition()
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };
            grid.Children.Add(new BoxView
            {
                VerticalOptions = LayoutOptions.Center,
                Color = Color.White
            });
            grid.Children.Add(new Label
            {
                Text = "Resultado Test Rápido",
                FontSize = 16,
                Margin = new Thickness(0, 10),
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            });
            grid.Children.Add(new BoxView
            {
                Color = Color.White
            }, 1, 0);
            Grid grid1 = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition()
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };
            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
            };
            
            StackLayout stack1 = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
            };
            CheckBox checkYes = new CheckBox();
            checkYes.HorizontalOptions = LayoutOptions.Center;
            checkYes.Color = (Color)Application.Current.Resources["Primary"];
            checkYes.CheckedChanged += (sender, e) =>
            {
                // Perform required operation after examining e.Value

                var list = stack1.Children;
                CheckBox checkNo = (CheckBox)list[1];


                //var item0 = item.NumeroPregunta;
                if (e.Value.ToString() == "True") { checkNo.IsChecked = false;
                    FormRecord.Resultado = "Positivo";
                }
                else
                {
                    FormRecord.Resultado = "";
                };
            };
            CheckBox checkNo = new CheckBox();
            checkNo.HorizontalOptions = LayoutOptions.Center;
            checkNo.CheckedChanged += (sender, e) =>
            {
                // Perform required operation after examining e.Value
                var list = stack.Children;
                CheckBox checkYes = (CheckBox)list[1];
                //var item0 = item.NumeroPregunta;
                if (e.Value.ToString() == "True")
                {
                    checkYes.IsChecked = false;
                    FormRecord.Resultado = "Negativo";
                }
                else
                {
                    FormRecord.Resultado = "";
                };
            };
            checkNo.Color = (Color)Application.Current.Resources["Primary"];
            
            stack.Children.Add(new Label
            {
                Text = "Positivo",
                FontSize = 16,
                //VerticalTextAlignment = TextAlignment.Center
            });
            
            stack.Children.Add(checkYes);
            
            stack1.Children.Add(new Label
            {
                Text = "Negativo",
                FontSize = 16,
                //VerticalTextAlignment = TextAlignment.Center
            });
            
            stack1.Children.Add(checkNo);

            grid1.Children.Add(stack,0,0);
            grid1.Children.Add(stack1,1,0);
            grid.Children.Add(grid1,1,0);
            BoxView boxView = new BoxView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Color = Color.Gray,
                HeightRequest = 1
            };
            Grid.SetRow(boxView, 1);
            General.Children.Add(grid);
            General.Children.Add(boxView);

            
        }

        private void SaveItemAnswer(string qtnId, int itemNbr,string resp)
        {
            switch (itemNbr)
            {
                case 1:
                    FormRecord.Res1 = resp;
                    FormRecord.Preg1 = qtnId;
                    break;
                case 2:
                    FormRecord.Res2 = resp;
                    FormRecord.Preg2 = qtnId;
                    break;
                case 3:
                    FormRecord.Res3 = resp;
                    FormRecord.Preg3 = qtnId;
                    break;
                case 4:
                    FormRecord.Res4 = resp;
                    FormRecord.Preg4 = qtnId;
                    break;
                default:
                    break;
            }
            IsButtonEnabled = FormRecord.FormFulfilledNoLocation;
        }
    }
}
