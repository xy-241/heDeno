﻿using heDenoDB.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace heDenoDB
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
        [OperationContract]
        List <Appointment> GetAppointmentsByPatientId(int id);

        [OperationContract]
        int CreateAppointment(DateTime startDateTime, DateTime endDateTime, int doctorId, int patientId);

        [OperationContract]
        int UpdateAppointment(int id, DateTime startDateTime, DateTime endDateTime, int doctorId, int patientId);

        [OperationContract]
        Appointment GetOneAppointment(int id);

        [OperationContract]
        List <Specialty> GetAllSpecialty();

        [OperationContract]
        List<Clinic> GetClinicBySpecialty(string specialty);

        [OperationContract]
        Clinic GetOneClinic(string clinic_name);

        [OperationContract]
        List<Doctor> GetDoctorByClinic(string clinic_id);

    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "heDenoDB.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
