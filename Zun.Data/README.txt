***Correr las migraciones en Zun.Datos

add-migration ZunMigracion1 -c ZunDbContext
update-database -context ZunDbContext
remove-migration -context ZunDbContext

add-migration TrazasMigracion1 -c TrazasDbContext
update-database -context TrazasDbContext
remove-migration -context TrazasDbContext