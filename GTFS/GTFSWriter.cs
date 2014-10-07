﻿// The MIT License (MIT)

// Copyright (c) 2014 Ben Abelshausen

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using GTFS.IO;
using System.Linq;
using System.Collections.Generic;
using GTFS.Entities;
using GTFS.Entities.Enumerations;
using System;

namespace GTFS
{
    /// <summary>
    /// A GTFS writer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GTFSWriter<T> where T : IGTFSFeed
    {
        /// <summary>
        /// Writes the given feed to the given target files.
        /// </summary>
        /// <param name="feed"></param>
        /// <param name="target"></param>
        public void Write(T feed, IEnumerable<IGTFSTargetFile> target)
        {
            // write files on-by-one.
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "agency"), feed.GetAgencies());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "calendar_dates"), feed.GetCalendarDates());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "calendar"), feed.GetCalendars());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "fare_attributes"), feed.GetFareAttributes());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "fare_rules"), feed.GetFareRules());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "feed_info"), feed.GetFeedInfo());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "frequencies"), feed.GetFrequencies());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "routes"), feed.GetRoutes());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "shapes"), feed.GetShapes());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "stops"), feed.GetStops());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "stop_times"), feed.GetStopTimes());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "tranfers"), feed.GetTransfers());
            this.Write(target.FirstOrDefault<IGTFSTargetFile>((x) => x.Name == "trips"), feed.GetTrips());
        }

        /// <summary>
        /// Writes all agencies to the given agencies file.
        /// </summary>
        /// <param name="agenciesFile"></param>
        /// <param name="agencies"></param>
        protected virtual void Write(IGTFSTargetFile agenciesFile, IEnumerable<Agency> agencies)
        {
            if (agenciesFile != null)
            {
                bool initialized = false;
                var data = new string[7];
                foreach (var agency in agencies)
                {
                    if (!initialized)
                    {
                        if (agenciesFile.Exists)
                        {
                            agenciesFile.Clear();
                        }

                        // write headers.
                        data[0] = "agency_id";
                        data[1] = "agency_name";
                        data[2] = "agency_url";
                        data[3] = "agency_timezone";
                        data[4] = "agency_lang";
                        data[5] = "agency_phone";
                        data[6] = "agency_fare_url";
                        agenciesFile.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("agency", "agency_id", agency.Id);
                    data[1] = this.WriteFieldString("agency", "agency_name", agency.Name);
                    data[2] = this.WriteFieldString("agency", "agency_url", agency.URL);
                    data[3] = this.WriteFieldString("agency", "agency_timezone", agency.Timezone);
                    data[4] = this.WriteFieldString("agency", "agency_lang", agency.LanguageCode);
                    data[5] = this.WriteFieldString("agency", "agency_phone", agency.Phone);
                    data[6] = this.WriteFieldString("agency", "agency_fare_url", agency.FareURL);
                    agenciesFile.Write(data);
                }
                agenciesFile.Close();
            }
        }

        /// <summary>
        /// Writes all calendar dates.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="entities"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<CalendarDate> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[3];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "service_id";
                        data[1] = "date";
                        data[2] = "exception_type";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("calendar_dates", "service_id", entity.ServiceId);
                    data[1] = this.WriteFieldDate("calendar_dates", "date", entity.Date);
                    data[2] = this.WriteFieldExceptionType("calendar_dates", "exception_type", entity.ExceptionType);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the calenders to the calenders file.
        /// </summary>
        /// <param name="calendarsFile"></param>
        /// <param name="calendars"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Calendar> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[10];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "service_id";
                        data[1] = "monday";
                        data[2] = "tuesday";
                        data[3] = "wednesday";
                        data[4] = "thursday";
                        data[5] = "friday";
                        data[6] = "saturday";
                        data[7] = "sunday";
                        data[8] = "start_date";
                        data[9] = "end_date";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("calendar", "service_id", entity.ServiceId);
                    data[1] = this.WriteFieldBool("calendar", "monday", entity.Monday);
                    data[2] = this.WriteFieldBool("calendar", "tuesday", entity.Tuesday);
                    data[3] = this.WriteFieldBool("calendar", "wednesday", entity.Wednesday);
                    data[4] = this.WriteFieldBool("calendar", "thursday", entity.Thursday);
                    data[5] = this.WriteFieldBool("calendar", "friday", entity.Friday);
                    data[6] = this.WriteFieldBool("calendar", "saturday", entity.Saturday);
                    data[7] = this.WriteFieldBool("calendar", "sunday", entity.Sunday);
                    data[8] = this.WriteFieldDate("calendar", "start_date", entity.StartDate);
                    data[9] = this.WriteFieldDate("calendar", "end_date", entity.EndDate);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the fare attributes.
        /// </summary>
        /// <param name="fareAttributesFile"></param>
        /// <param name="fareAttributes"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<FareAttribute> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[6];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "fare_id";
                        data[1] = "price";
                        data[2] = "currency_type";
                        data[3] = "payment_method";
                        data[4] = "transfers";
                        data[5] = "transfer_duration";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("fare_attributes", "fare_id", entity.FareId);
                    data[1] = this.WriteFieldString("fare_attributes", "price", entity.Price);
                    data[2] = this.WriteFieldString("fare_attributes", "currency_type", entity.CurrencyType);
                    data[3] = this.WriteFieldPaymentMethod("fare_attributes", "payment_method", entity.PaymentMethod);
                    data[4] = this.WriteFieldUint("fare_attributes", "transfers", entity.Transfers);
                    data[5] = this.WriteFieldString("fare_attributes", "transfer_duration", entity.TransferDuration);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the fare rules.
        /// </summary>
        /// <param name="fareRulesFile"></param>
        /// <param name="fareRule"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<FareRule> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[5];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "fare_id";
                        data[1] = "route_id";
                        data[2] = "origin_id";
                        data[3] = "destination_id";
                        data[4] = "contains_id";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("fare_rules", "fare_id", entity.FareId);
                    data[1] = this.WriteFieldString("fare_rules", "route_id", entity.RouteId);
                    data[2] = this.WriteFieldString("fare_rules", "origin_id", entity.OriginId);
                    data[3] = this.WriteFieldString("fare_rules", "destination_id", entity.DestinationId);
                    data[4] = this.WriteFieldString("fare_rules", "contains_id", entity.ContainsId);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the feed info.
        /// </summary>
        /// <param name="feedInfoFile"></param>
        /// <param name="feedInfo"></param>
        protected virtual void Write(IGTFSTargetFile file, FeedInfo entity)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[6];
                if (!initialized)
                {
                    if (file.Exists)
                    {
                        file.Clear();
                    }

                    // write headers.
                    data[0] = "feed_publisher_name";
                    data[1] = "feed_publisher_url";
                    data[2] = "feed_lang";
                    data[3] = "feed_start_date";
                    data[4] = "feed_end_date";
                    data[5] = "feed_version";
                    file.Write(data);
                    initialized = true;
                }

                // write agency details.
                data[0] = this.WriteFieldString("feed_info", "feed_publisher_name", entity.PublisherName);
                data[1] = this.WriteFieldString("feed_info", "feed_publisher_url", entity.PublisherUrl);
                data[2] = this.WriteFieldString("feed_info", "feed_lang", entity.Lang);
                data[3] = this.WriteFieldString("feed_info", "feed_start_date", entity.StartDate);
                data[4] = this.WriteFieldString("feed_info", "feed_end_date", entity.EndDate);
                data[5] = this.WriteFieldString("feed_info", "feed_version", entity.Version);
                file.Write(data);
                file.Close();
            }
        }

        /// <summary>
        /// Writes the frequencies.
        /// </summary>
        /// <param name="frequenciesFile"></param>
        /// <param name="frequencies"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Frequency> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[5];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "trip_id";
                        data[1] = "start_time";
                        data[2] = "end_time";
                        data[3] = "headway_secs";
                        data[4] = "exact_times";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("frequency", "trip_id", entity.TripId);
                    data[1] = this.WriteFieldString("frequency", "start_time", entity.StartTime);
                    data[2] = this.WriteFieldString("frequency", "end_time", entity.EndTime);
                    data[3] = this.WriteFieldString("frequency", "headway_secs", entity.HeadwaySecs);
                    data[4] = this.WriteFieldBool("frequency", "exact_times", entity.ExactTimes);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the routes.
        /// </summary>
        /// <param name="routesFile"></param>
        /// <param name="route"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Route> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[9];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "route_id";
                        data[1] = "agency_id";
                        data[2] = "route_short_name";
                        data[3] = "route_long_name";
                        data[4] = "route_desc";
                        data[5] = "route_type";
                        data[6] = "route_url";
                        data[7] = "route_color";
                        data[8] = "route_text_color";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("routes", "route_id", entity.Id);
                    data[1] = this.WriteFieldString("routes", "agency_id", entity.AgencyId);
                    data[2] = this.WriteFieldString("routes", "route_short_name", entity.ShortName);
                    data[3] = this.WriteFieldString("routes", "route_long_name", entity.LongName);
                    data[4] = this.WriteFieldString("routes", "route_desc", entity.Description);
                    data[5] = this.WriteFieldRouteType("routes", "route_type", entity.Type);
                    data[6] = this.WriteFieldString("routes", "route_url", entity.Url);
                    data[7] = this.WriteFieldColor("routes", "route_color", entity.Color);
                    data[8] = this.WriteFieldColor("routes", "route_text_color", entity.TextColor);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the shapes.
        /// </summary>
        /// <param name="shapesFile"></param>
        /// <param name="shapes"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Shape> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[5];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "shape_id";
                        data[1] = "shape_pt_lat";
                        data[2] = "shape_pt_lon";
                        data[3] = "shape_pt_sequence";
                        data[4] = "shape_dist_traveled";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("shapes", "shape_id", entity.Id);
                    data[1] = this.WriteFieldDouble("shapes", "shape_pt_lat", entity.Latitude);
                    data[2] = this.WriteFieldDouble("shapes", "shape_pt_lon", entity.Longitude);
                    data[3] = this.WriteFieldUint("shapes", "shape_pt_sequence", entity.Sequence);
                    data[4] = this.WriteFieldDouble("shapes", "shape_dist_traveled", entity.DistanceTravelled);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the stops.
        /// </summary>
        /// <param name="stopsFile"></param>
        /// <param name="stops"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Stop> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[12];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "stop_id";
                        data[1] = "stop_code";
                        data[2] = "stop_name";
                        data[3] = "stop_desc";
                        data[4] = "stop_lat";
                        data[5] = "stop_lon";
                        data[6] = "zone_id";
                        data[7] = "stop_url";
                        data[8] = "location_type";
                        data[9] = "parent_station";
                        data[10] = "stop_timezone";
                        data[11] = "wheelchair_boarding";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("stops", "stop_id", entity.Id);
                    data[1] = this.WriteFieldString("stops", "stop_code", entity.Code);
                    data[2] = this.WriteFieldString("stops", "stop_name", entity.Name);
                    data[3] = this.WriteFieldString("stops", "stop_desc", entity.Description);
                    data[4] = this.WriteFieldDouble("stops", "stop_lat", entity.Latitude);
                    data[5] = this.WriteFieldDouble("stops", "stop_lon", entity.Longitude);
                    data[6] = this.WriteFieldString("stops", "zone_id", entity.Zone);
                    data[7] = this.WriteFieldString("stops", "stop_url", entity.Url);
                    data[8] = this.WriteFieldLocationType("stops", "location_type", entity.LocationType);
                    data[9] = this.WriteFieldString("stops", "parent_station", entity.ParentStation);
                    data[10] = this.WriteFieldString("stops", "stop_timezone", entity.Timezone);
                    data[11] = this.WriteFieldString("stops", "wheelchair_boarding", entity.WheelchairBoarding);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the stop times.
        /// </summary>
        /// <param name="stopTimesFile"></param>
        /// <param name="stopTimes"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<StopTime> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[9];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "trip_id";
                        data[1] = "arrival_time";
                        data[2] = "departure_time";
                        data[3] = "stop_id";
                        data[4] = "stop_sequence";
                        data[5] = "stop_headsign";
                        data[6] = "pickup_type";
                        data[7] = "drop_off_type";
                        data[8] = "shape_dist_traveled";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("stop_times", "trip_id", entity.TripId);
                    data[1] = this.WriteFieldTimeOfDay("stop_times", "arrival_time", entity.ArrivalTime);
                    data[2] = this.WriteFieldTimeOfDay("stop_times", "departure_time", entity.DepartureTime);
                    data[3] = this.WriteFieldString("stop_times", "stop_id", entity.StopId);
                    data[4] = this.WriteFieldUint("stop_times", "stop_sequence", entity.StopSequence);
                    data[5] = this.WriteFieldString("stop_times", "stop_headsign", entity.StopHeadsign);
                    data[6] = this.WriteFieldPickupType("stop_times", "pickup_type", entity.PickupType);
                    data[7] = this.WriteFieldDropOffType("stop_times", "drop_off_type", entity.DropOffType);
                    data[8] = this.WriteFieldString("stop_times", "shape_dist_traveled", entity.ShapeDistTravelled);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the transfers.
        /// </summary>
        /// <param name="transfersFile"></param>
        /// <param name="transfers"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Transfer> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[4];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "from_stop_id";
                        data[1] = "to_stop_id";
                        data[2] = "transfer_type";
                        data[3] = "min_transfer_time";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("transfers", "from_stop_id", entity.FromStop.Id);
                    data[1] = this.WriteFieldString("transfers", "to_stop_id", entity.ToStop.Id);
                    data[2] = this.WriteFieldTransferType("transfers", "transfer_type", entity.TransferType);
                    data[3] = this.WriteFieldString("transfers", "min_transfer_time", entity.MinimumTransferTime);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes the trips.
        /// </summary>
        /// <param name="tripsFile"></param>
        /// <param name="trips"></param>
        protected virtual void Write(IGTFSTargetFile file, IEnumerable<Trip> entities)
        {
            if (file != null)
            {
                bool initialized = false;
                var data = new string[9];
                foreach (var entity in entities)
                {
                    if (!initialized)
                    {
                        if (file.Exists)
                        {
                            file.Clear();
                        }

                        // write headers.
                        data[0] = "trip_id";
                        data[1] = "route_id";
                        data[2] = "service_id";
                        data[3] = "trip_headsign";
                        data[4] = "trip_short_name";
                        data[5] = "direction_id";
                        data[6] = "block_id";
                        data[7] = "shape_id";
                        data[8] = "wheelchair_accessible";
                        file.Write(data);
                        initialized = true;
                    }

                    // write agency details.
                    data[0] = this.WriteFieldString("trips", "trip_id", entity.Id);
                    data[1] = this.WriteFieldString("trips", "route_id", entity.RouteId);
                    data[2] = this.WriteFieldString("trips", "service_id", entity.ServiceId);
                    data[3] = this.WriteFieldString("trips", "trip_headsign", entity.Headsign);
                    data[4] = this.WriteFieldString("trips", "trip_short_name", entity.ShortName);
                    data[5] = this.WriteFieldDirectionType("trips", "direction_id", entity.Direction);
                    data[6] = this.WriteFieldString("trips", "block_id", entity.BlockId);
                    data[7] = this.WriteFieldString("trips", "shape_id", entity.ShapeId);
                    data[8] = this.WriteFieldAccessibilityType("trips", "wheelchair_accessible", entity.AccessibilityType);
                    file.Write(data);
                }
                file.Close();
            }
        }

        /// <summary>
        /// Writes a string-field.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string WriteFieldString(string name, string fieldName, string value)
        {
            return value;
        }

        /// <summary>
        /// Writes the exception type.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="exceptionType"></param>
        /// <returns></returns>
        private string WriteFieldExceptionType(string name, string fieldName, ExceptionType value)
        {
            //A value of 1 indicates that service has been added for the specified date.
            //A value of 2 indicates that service has been removed for the specified date.

            switch (value)
            {
                case ExceptionType.Added:
                    return "1";
                case ExceptionType.Removed:
                    return "2";
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the date.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private string WriteFieldDate(string name, string fieldName, DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd");
        }

        /// <summary>
        /// Writes the bool.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        private string WriteFieldBool(string name, string fieldName, bool? value)
        {
            if (value.HasValue)
            {
                if(value.Value)
                {
                    return "1";
                }
                return "0";
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the uint.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldUint(string name, string fieldName, uint? value)
        {
            if(value.HasValue)
            {
                return value.Value.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the payment method.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="paymentMethodType"></param>
        /// <returns></returns>
        private string WriteFieldPaymentMethod(string name, string fieldName, PaymentMethodType value)
        {
            //0 - Fare is paid on board.
            //1 - Fare must be paid before boarding.

            switch (value)
            {
                case PaymentMethodType.OnBoard:
                    return "0";
                case PaymentMethodType.BeforeBoarding:
                    return "1";
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes a color.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldColor(string name, string fieldName, int? value)
        {
            if(value.HasValue)
            {
                var r = (short)(((uint)value.Value >> 16) % 256);
                var g = (short)(((uint)value.Value >> 8) % 256);
                var b = (short)((uint)value.Value % 256);

                return string.Format("#{0}{1}{2}",
                                     r.ToString("X2"),
                                     g.ToString("X2"),
                                     b.ToString("X2"));
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the route type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldRouteType(string name, string fieldName, RouteType value)
        {           
            //0 - Tram, Streetcar, Light rail. Any light rail or street level system within a metropolitan area.
            //1 - Subway, Metro. Any underground rail system within a metropolitan area.
            //2 - Rail. Used for intercity or long-distance travel.
            //3 - Bus. Used for short- and long-distance bus routes.
            //4 - Ferry. Used for short- and long-distance boat service.
            //5 - Cable car. Used for street-level cable cars where the cable runs beneath the car.
            //6 - Gondola, Suspended cable car. Typically used for aerial cable cars where the car is suspended from the cable.
            //7 - Funicular. Any rail system designed for steep inclines.

            switch(value)
            {
                case RouteType.Tram:
                    return "0";
                case RouteType.SubwayMetro:
                    return "1";
                case RouteType.Rail:
                    return "2";
                case RouteType.Bus:
                    return "3";
                case RouteType.Ferry:
                    return "4";
                case RouteType.CableCar:
                    return "5";
                case RouteType.Gondola:
                    return "6";
                case RouteType.Funicular:
                    return "7";
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes a double.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldDouble(string name, string fieldName, double? value)
        {
            if(value.HasValue)
            {
                return value.Value.ToString(System.Globalization.CultureInfo.InstalledUICulture);
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the location type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldLocationType(string name, string fieldName, LocationType? value)
        {
            if(value.HasValue)
            {
                switch (value.Value)
                {
                    case LocationType.Stop:
                        return "0";
                    case LocationType.Station:
                        return "1";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the drop off type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldDropOffType(string name, string fieldName, DropOffType? value)
        {
            if(value.HasValue)
            {
                switch (value.Value)
                {
                    case DropOffType.Regular:
                        return "0";
                    case DropOffType.NoPickup:
                        return "1";
                    case DropOffType.PhoneForPickup:
                        return "2";
                    case DropOffType.DriverForPickup:
                        return "3";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes the pickup type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldPickupType(string name, string fieldName, PickupType? value)
        {
            if(value.HasValue)
            {
                switch (value.Value)
                {
                    case PickupType.Regular:
                        return "0";
                    case PickupType.NoPickup:
                        return "1";
                    case PickupType.PhoneForPickup:
                        return "2";
                    case PickupType.DriverForPickup:
                        return "3";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes a timeofday.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldTimeOfDay(string name, string fieldName, TimeOfDay value)
        {
            return string.Format("{0}:{1}:{2}",
                value.Hours.ToString("00"),
                value.Minutes.ToString("00"),
                value.Seconds.ToString("00"));
        }

        /// <summary>
        /// Writes a transfertime.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldTransferType(string name, string fieldName, TransferType value)
        {
            return string.Empty;
        }

        /// <summary>
        /// Writes an accessibility type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldAccessibilityType(string name, string fieldName, WheelchairAccessibilityType? value)
        {
            if (value.HasValue)
            {
                //0 (or empty) - indicates that there is no accessibility information for the trip
                //1 - indicates that the vehicle being used on this particular trip can accommodate at least one rider in a wheelchair
                //2 - indicates that no riders in wheelchairs can be accommodated on this trip

                switch (value.Value)
                {
                    case WheelchairAccessibilityType.NoInformation:
                        return "0";
                    case WheelchairAccessibilityType.SomeAccessibility:
                        return "1";
                    case WheelchairAccessibilityType.NoAccessibility:
                        return "2";
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes a direction type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string WriteFieldDirectionType(string name, string fieldName, DirectionType? value)
        {
            if (value.HasValue)
            {

                //0 - travel in one direction (e.g. outbound travel)
                //1 - travel in the opposite direction (e.g. inbound travel)

                switch (value.Value)
                {
                    case DirectionType.OneDirection:
                        return "0";
                    case DirectionType.OppositeDirection:
                        return "1";
                }
            }
            return string.Empty;
        }
    }
}