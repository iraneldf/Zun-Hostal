add-migration "<nombreMigracion>" -c ZunDbContext
update-database -context ZunDbContext
remove-migration -context ZunDbContext