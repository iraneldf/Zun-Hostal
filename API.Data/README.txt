***Correr las migraciones en API.Data

add-migration ApiMigracion1 -c ApiDbContext
update-database -context ApiDbContext
remove-migration -context ApiDbContext

add-migration TrazasMigracion1 -c TrazasDbContext
update-database -context TrazasDbContext
remove-migration -context TrazasDbContext