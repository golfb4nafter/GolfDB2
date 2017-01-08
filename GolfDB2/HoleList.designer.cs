﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GolfDB2
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="GolfDB20161207-01")]
	public partial class HoleListDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertHoleList(HoleList instance);
    partial void UpdateHoleList(HoleList instance);
    partial void DeleteHoleList(HoleList instance);
    partial void InsertHole(Hole instance);
    partial void UpdateHole(Hole instance);
    partial void DeleteHole(Hole instance);
    #endregion
		
		public HoleListDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["GolfDB20161207_01ConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public HoleListDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public HoleListDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public HoleListDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public HoleListDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<HoleList> HoleLists
		{
			get
			{
				return this.GetTable<HoleList>();
			}
		}
		
		public System.Data.Linq.Table<Hole> Holes
		{
			get
			{
				return this.GetTable<Hole>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.HoleList")]
	public partial class HoleList : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _CourseId;
		
		private string _Label;
		
		private string _HoleList1;
		
		private string _BList;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCourseIdChanging(int value);
    partial void OnCourseIdChanged();
    partial void OnLabelChanging(string value);
    partial void OnLabelChanged();
    partial void OnHoleList1Changing(string value);
    partial void OnHoleList1Changed();
    partial void OnBListChanging(string value);
    partial void OnBListChanged();
    #endregion
		
		public HoleList()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CourseId", DbType="Int NOT NULL")]
		public int CourseId
		{
			get
			{
				return this._CourseId;
			}
			set
			{
				if ((this._CourseId != value))
				{
					this.OnCourseIdChanging(value);
					this.SendPropertyChanging();
					this._CourseId = value;
					this.SendPropertyChanged("CourseId");
					this.OnCourseIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Label", DbType="NVarChar(128) NOT NULL", CanBeNull=false)]
		public string Label
		{
			get
			{
				return this._Label;
			}
			set
			{
				if ((this._Label != value))
				{
					this.OnLabelChanging(value);
					this.SendPropertyChanging();
					this._Label = value;
					this.SendPropertyChanged("Label");
					this.OnLabelChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="HoleList", Storage="_HoleList1", DbType="NVarChar(256) NOT NULL", CanBeNull=false)]
		public string HoleList1
		{
			get
			{
				return this._HoleList1;
			}
			set
			{
				if ((this._HoleList1 != value))
				{
					this.OnHoleList1Changing(value);
					this.SendPropertyChanging();
					this._HoleList1 = value;
					this.SendPropertyChanged("HoleList1");
					this.OnHoleList1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BList", DbType="NVarChar(256)")]
		public string BList
		{
			get
			{
				return this._BList;
			}
			set
			{
				if ((this._BList != value))
				{
					this.OnBListChanging(value);
					this.SendPropertyChanging();
					this._BList = value;
					this.SendPropertyChanged("BList");
					this.OnBListChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Hole")]
	public partial class Hole : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Id;
		
		private int _CourseId;
		
		private int _Nine;
		
		private int _Number;
		
		private string _PhotoUrl;
		
		private string _Description;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(int value);
    partial void OnIdChanged();
    partial void OnCourseIdChanging(int value);
    partial void OnCourseIdChanged();
    partial void OnNineChanging(int value);
    partial void OnNineChanged();
    partial void OnNumberChanging(int value);
    partial void OnNumberChanged();
    partial void OnPhotoUrlChanging(string value);
    partial void OnPhotoUrlChanged();
    partial void OnDescriptionChanging(string value);
    partial void OnDescriptionChanged();
    #endregion
		
		public Hole()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CourseId", DbType="Int NOT NULL")]
		public int CourseId
		{
			get
			{
				return this._CourseId;
			}
			set
			{
				if ((this._CourseId != value))
				{
					this.OnCourseIdChanging(value);
					this.SendPropertyChanging();
					this._CourseId = value;
					this.SendPropertyChanged("CourseId");
					this.OnCourseIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Nine", DbType="Int NOT NULL")]
		public int Nine
		{
			get
			{
				return this._Nine;
			}
			set
			{
				if ((this._Nine != value))
				{
					this.OnNineChanging(value);
					this.SendPropertyChanging();
					this._Nine = value;
					this.SendPropertyChanged("Nine");
					this.OnNineChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Number", DbType="Int NOT NULL")]
		public int Number
		{
			get
			{
				return this._Number;
			}
			set
			{
				if ((this._Number != value))
				{
					this.OnNumberChanging(value);
					this.SendPropertyChanging();
					this._Number = value;
					this.SendPropertyChanged("Number");
					this.OnNumberChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PhotoUrl", DbType="NVarChar(MAX)")]
		public string PhotoUrl
		{
			get
			{
				return this._PhotoUrl;
			}
			set
			{
				if ((this._PhotoUrl != value))
				{
					this.OnPhotoUrlChanging(value);
					this.SendPropertyChanging();
					this._PhotoUrl = value;
					this.SendPropertyChanged("PhotoUrl");
					this.OnPhotoUrlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Description", DbType="NVarChar(MAX)")]
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				if ((this._Description != value))
				{
					this.OnDescriptionChanging(value);
					this.SendPropertyChanging();
					this._Description = value;
					this.SendPropertyChanged("Description");
					this.OnDescriptionChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
