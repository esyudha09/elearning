--update mas_data_pegawai set email = replace(email,'@indonesiapower.co.id','@plnindonesiapower.co.id');
--commit;

--
select  email from mas_data_pegawai where email like '%@indonesiapower.co.id%'
union all
select  email  from tbl_user where email like '%@indonesiapower.co.id%';


--delete from tbl_user where id in (
select id from(
select row_number() over (partition by username order by username) as no,id,nama,username, email,jabatan,jenjang,unit,role_id from tbl_user where username in(
    select username from (
        SELECT username,username, email,jabatan,jenjang,unit,role_id, COUNT(username) a
        FROM tbl_user
        GROUP BY username, email,jabatan,jenjang,unit,role_id
        HAVING COUNT(username) > 1 order by a desc
        )
)    --order by username) where no = 2
--);
--commit;



select * from tbl_user where username like '%gibran%' 