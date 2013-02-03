﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProxySearch.Console.GeoIPService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.webservicex.net/", ConfigurationName="GeoIPService.GeoIPServiceSoap")]
    public interface GeoIPServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.webservicex.net/GetGeoIP", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        ProxySearch.Console.GeoIPService.GeoIP GetGeoIP(string IPAddress);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.webservicex.net/GetGeoIPContext", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        ProxySearch.Console.GeoIPService.GeoIP GetGeoIPContext();
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.webservicex.net/")]
    public partial class GeoIP : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int returnCodeField;
        
        private string ipField;
        
        private string returnCodeDetailsField;
        
        private string countryNameField;
        
        private string countryCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int ReturnCode {
            get {
                return this.returnCodeField;
            }
            set {
                this.returnCodeField = value;
                this.RaisePropertyChanged("ReturnCode");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string IP {
            get {
                return this.ipField;
            }
            set {
                this.ipField = value;
                this.RaisePropertyChanged("IP");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ReturnCodeDetails {
            get {
                return this.returnCodeDetailsField;
            }
            set {
                this.returnCodeDetailsField = value;
                this.RaisePropertyChanged("ReturnCodeDetails");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string CountryName {
            get {
                return this.countryNameField;
            }
            set {
                this.countryNameField = value;
                this.RaisePropertyChanged("CountryName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string CountryCode {
            get {
                return this.countryCodeField;
            }
            set {
                this.countryCodeField = value;
                this.RaisePropertyChanged("CountryCode");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface GeoIPServiceSoapChannel : ProxySearch.Console.GeoIPService.GeoIPServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GeoIPServiceSoapClient : System.ServiceModel.ClientBase<ProxySearch.Console.GeoIPService.GeoIPServiceSoap>, ProxySearch.Console.GeoIPService.GeoIPServiceSoap {
        
        public GeoIPServiceSoapClient() {
        }
        
        public GeoIPServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public GeoIPServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GeoIPServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public GeoIPServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public ProxySearch.Console.GeoIPService.GeoIP GetGeoIP(string IPAddress) {
            return base.Channel.GetGeoIP(IPAddress);
        }
        
        public ProxySearch.Console.GeoIPService.GeoIP GetGeoIPContext() {
            return base.Channel.GetGeoIPContext();
        }
    }
}
