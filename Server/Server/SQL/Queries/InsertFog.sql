INSERT INTO dbo.fogs(input_type, return_type, member_count, times_used) output inserted.fog_id
VALUES(@input_type, @return_type, @member_count, 0);