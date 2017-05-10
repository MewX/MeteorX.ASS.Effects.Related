﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeteorX.AssTools.KaraokeApp
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="AvsMask")]
	public partial class MaskDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region 可扩展性方法定义
    partial void OnCreated();
    partial void InsertMask(Mask instance);
    partial void UpdateMask(Mask instance);
    partial void DeleteMask(Mask instance);
    #endregion
		
		public MaskDataContext() : 
				base(global::MeteorX.AssTools.KaraokeApp.Properties.Settings.Default.AvsMaskConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public MaskDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MaskDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MaskDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public MaskDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Mask> Masks
		{
			get
			{
				return this.GetTable<Mask>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Mask")]
	public partial class Mask : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _MaskId;
		
		private int _PlayResX;
		
		private int _PlayResY;
		
		private string _Style;
		
		private System.Data.Linq.Binary _Data;
		
		private int _X;
		
		private int _Y;
		
		private string _Str;
		
    #region 可扩展性方法定义
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnMaskIdChanging(int value);
    partial void OnMaskIdChanged();
    partial void OnPlayResXChanging(int value);
    partial void OnPlayResXChanged();
    partial void OnPlayResYChanging(int value);
    partial void OnPlayResYChanged();
    partial void OnStyleChanging(string value);
    partial void OnStyleChanged();
    partial void OnDataChanging(System.Data.Linq.Binary value);
    partial void OnDataChanged();
    partial void OnXChanging(int value);
    partial void OnXChanged();
    partial void OnYChanging(int value);
    partial void OnYChanged();
    partial void OnStrChanging(string value);
    partial void OnStrChanged();
    #endregion
		
		public Mask()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MaskId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int MaskId
		{
			get
			{
				return this._MaskId;
			}
			set
			{
				if ((this._MaskId != value))
				{
					this.OnMaskIdChanging(value);
					this.SendPropertyChanging();
					this._MaskId = value;
					this.SendPropertyChanged("MaskId");
					this.OnMaskIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PlayResX", DbType="Int NOT NULL")]
		public int PlayResX
		{
			get
			{
				return this._PlayResX;
			}
			set
			{
				if ((this._PlayResX != value))
				{
					this.OnPlayResXChanging(value);
					this.SendPropertyChanging();
					this._PlayResX = value;
					this.SendPropertyChanged("PlayResX");
					this.OnPlayResXChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PlayResY", DbType="Int NOT NULL")]
		public int PlayResY
		{
			get
			{
				return this._PlayResY;
			}
			set
			{
				if ((this._PlayResY != value))
				{
					this.OnPlayResYChanging(value);
					this.SendPropertyChanging();
					this._PlayResY = value;
					this.SendPropertyChanged("PlayResY");
					this.OnPlayResYChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Style", DbType="VarChar(500) NOT NULL", CanBeNull=false)]
		public string Style
		{
			get
			{
				return this._Style;
			}
			set
			{
				if ((this._Style != value))
				{
					this.OnStyleChanging(value);
					this.SendPropertyChanging();
					this._Style = value;
					this.SendPropertyChanged("Style");
					this.OnStyleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Data", DbType="VarBinary(MAX) NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public System.Data.Linq.Binary Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				if ((this._Data != value))
				{
					this.OnDataChanging(value);
					this.SendPropertyChanging();
					this._Data = value;
					this.SendPropertyChanged("Data");
					this.OnDataChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_X", DbType="Int NOT NULL")]
		public int X
		{
			get
			{
				return this._X;
			}
			set
			{
				if ((this._X != value))
				{
					this.OnXChanging(value);
					this.SendPropertyChanging();
					this._X = value;
					this.SendPropertyChanged("X");
					this.OnXChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Y", DbType="Int NOT NULL")]
		public int Y
		{
			get
			{
				return this._Y;
			}
			set
			{
				if ((this._Y != value))
				{
					this.OnYChanging(value);
					this.SendPropertyChanging();
					this._Y = value;
					this.SendPropertyChanged("Y");
					this.OnYChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Str", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Str
		{
			get
			{
				return this._Str;
			}
			set
			{
				if ((this._Str != value))
				{
					this.OnStrChanging(value);
					this.SendPropertyChanging();
					this._Str = value;
					this.SendPropertyChanged("Str");
					this.OnStrChanged();
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