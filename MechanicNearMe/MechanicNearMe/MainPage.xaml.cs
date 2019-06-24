using Plugin.Permissions.Abstractions;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;
using MechanicNearMe.Models;

namespace MechanicNearMe
{
    public partial class MainPage : ContentPage
    {
        Plugin.Geolocator.Abstractions.Position position;
        IList<Result> result;
        public MainPage()
        {
            InitializeComponent();
            GetCurrentLocation();

        }

        public async void GetCurrentLocation()
        {

            try
            {
                var hasPermission = await Utils.CheckPermissions(Permission.Location);
                if (!hasPermission)
                {
                    position = new Plugin.Geolocator.Abstractions.Position() { Latitude = 0.0, Longitude = 0.0 };
                    // return false; 
                }
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                //LabelLocation.Text = "Getting gps...";

                var CurrentPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null, false);

                if (CurrentPosition == null)
                {
                    position = new Plugin.Geolocator.Abstractions.Position() { Latitude = 0.0, Longitude = 0.0 };
                    // return false;
                }
                position = new Plugin.Geolocator.Abstractions.Position() { Latitude = CurrentPosition.Latitude, Longitude = CurrentPosition.Longitude };
                //LabelLocation.Text = position.Latitude + "," + position.Longitude;
                GenerateMap(position);
                await PlotPlaces(position);
                //return true;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
                position = new Plugin.Geolocator.Abstractions.Position() { Latitude = 0.0, Longitude = 0.0 };
                // return false;
            }
        }

        private async Task<bool> PlotPlaces(Plugin.Geolocator.Abstractions.Position position)
        {
            GMapApi api = new GMapApi();
            result = api.FetchData(position.Latitude.ToString(), position.Longitude.ToString(), "5000", "mechanic");
            foreach (var item in result)
            {
                var pin = new Pin
                {
                    Type = PinType.SavedPin,
                    Position = new Xamarin.Forms.Maps.Position(
                                                        Convert.ToDouble(item.geometry.location.lat), Convert.ToDouble(item.geometry.location.lng)
                                                        ),
                    Label = item.name,
                    Address = item.vicinity
                };
                MyMap.Pins.Add(pin);
            }
            MyMap.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        new Xamarin.Forms.Maps.Position(
                            position.Latitude, position.Longitude
                            ), Distance.FromMiles(3)
                            )
                          );
            return true;
        }

        private void GenerateMap(Plugin.Geolocator.Abstractions.Position currentPosition)
        {
            MyMap.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        new Xamarin.Forms.Maps.Position(
                            currentPosition.Latitude, currentPosition.Longitude
                            ), Distance.FromMiles(1)
                            )
                          );
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = new Xamarin.Forms.Maps.Position(
                                                        currentPosition.Latitude, currentPosition.Longitude
                                                        ),
                Label = "You Are Here",
                Address = currentPosition.Latitude + "," + currentPosition.Longitude
            };
            MyMap.Pins.Add(pin);
        }


    }
}
