create database BDPruebaMateriaGris;
go
use BDPruebaMateriaGris;
go

create table Usuario(
	IdUsuario int primary key identity(1,1),
	Nombre nvarchar(100) unique not null
);
go

create table Tarea (
	IdTarea int primary key identity(1,1),
	IdUsuario int foreign key references Usuario(IdUsuario),
	Nombre nvarchar(50) unique not null,
	Descripcion nvarchar(1000) not null,
	Estado bit default 1,
	FechaCreacion datetime default getdate(),
	FechaActualizacion datetime default getdate(),
);
go 

create procedure AgregarTarea
	@IdUsuario int,
	@Nombre nvarchar(50),
	@Descripcion nvarchar(1000),
	@Resultado bit output,
	@Mensaje nvarchar(1000) output
as
begin
	begin try
		declare @Contador int
		if not exists (select 1 from Usuario where IdUsuario = @IdUsuario)
		begin
			set @Mensaje = 'El usuario asignado no existe';
			set @Resultado = 0;
		end
		else if @Descripcion is null or @Descripcion = ''
		begin
			set @Mensaje = 'Tiene que agregar una descripción a la tarea';
			set @Resultado = 0;
		end
		else
		begin
			if @Nombre is null or @Nombre = ''
			begin
				set @Contador = (select COUNT(1) from Tarea);
				set @Nombre = 'Tarea ' + CAST(@Contador as nvarchar(10));
			end
			insert into Tarea (IdUsuario, Nombre, Descripcion)
			values (@IdUsuario, @Nombre, @Descripcion)
			set @Mensaje = 'Tarea agregada correctamente';
			set @Resultado = 1;
		end
	end try
	begin catch
		declare @ErrorMessage nvarchar(400);
        declare @ErrorProcedure nvarchar(200);
        set @ErrorMessage = ERROR_MESSAGE();
        set @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), 'Procedimiento desconocido');
        set @Mensaje = @ErrorProcedure + ' - ' + @ErrorMessage;
		set @Resultado = 0
	end catch
end
go

create procedure ListaTarea
as
begin
	select * from Tarea
end
go

create procedure ModificarTarea
	@IdTarea int,
	@IdUsuario int,
	@Nombre nvarchar(50),
	@Descripcion nvarchar(1000),
	@Estado bit,
	@Resultado bit output,
	@Mensaje nvarchar(1000) output
as
begin
	begin try
		if not exists (select 1 from Tarea where IdTarea = @IdTarea)
		begin
			set @Mensaje = 'Tarea no encontrada';
			set @Resultado = 0;
		end
		else
		begin
			declare @Contador int
			if @Descripcion is null or @Descripcion = ''
			begin
				set @Descripcion = (select Descripcion from Tarea where IdTarea = @IdTarea)
			end
			if @Nombre is null or @Nombre = ''
			begin
				set @Contador = (select COUNT(1) from Tarea);
				set @Nombre = 'Tarea ' + CAST(@Contador as nvarchar(10));
			end
			update Tarea set Descripcion = @Descripcion, IdUsuario = @IdUsuario, Nombre = @Nombre, Estado = @Estado where IdTarea = @IdTarea;
			set @Mensaje = 'Tarea actualizada correctamente';
			set @Resultado = 1;
		end
	end try
	begin catch
		declare @ErrorMessage nvarchar(400);
        declare @ErrorProcedure nvarchar(200);
        set @ErrorMessage = ERROR_MESSAGE();
        set @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), 'Procedimiento desconocido');
        set @Mensaje = @ErrorProcedure + ' - ' + @ErrorMessage;
		set @Resultado = 0
	end catch
end
go

create procedure EliminarTarea
	@IdTarea int,
	@Resultado bit output,
	@Mensaje nvarchar(1000) output
as
begin
	begin try
		if not exists (select 1 from Tarea where IdTarea = @IdTarea)
		begin
			set @Mensaje = 'Tarea no encontrada';
			set @Resultado = 0;
		end
		else
		begin
			delete Tarea where IdTarea = @IdTarea;
			set @Mensaje = 'Tarea eliminada correctamente';
			set @Resultado = 1;
		end
	end try
	begin catch
		declare @ErrorMessage nvarchar(400);
        declare @ErrorProcedure nvarchar(200);
        set @ErrorMessage = ERROR_MESSAGE();
        set @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), 'Procedimiento desconocido');
        set @Mensaje = @ErrorProcedure + ' - ' + @ErrorMessage;
		set @Resultado = 0
	end catch
end
go

create procedure ListaUsuario
as
begin
	select * from Usuario
end
go

insert Usuario (Nombre) values ('Usuario 1')
insert Usuario (Nombre) values ('Usuario 2')
insert Usuario (Nombre) values ('Usuario 3')
insert Usuario (Nombre) values ('Usuario 4')