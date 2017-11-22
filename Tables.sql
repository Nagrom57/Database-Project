

Create Table Project.Movies (
	MovieID		int				Primary Key identity
	, Title			varchar(100)	not null
	, ContentRating	varchar(8)
	, ReleaseDate	date
	, Runtime		int
)

create table Project.Trailer (
	TrailerID			int			Primary Key Identity
	, MovieID			int			Foreign Key
			References Project.Movies (MovieID)
	, TrailerSequence	int	not null
	, URL				varchar(400)
)

create table Project.Languages (
	LanguageID		int				Primary Key Identity
	, Language		varchar(15)		not null
)

create table Project.MovieLanguages (
	MovieLanguageID		int		Primary Key
	, LanguageID		int		Foreign Key
			References Project.Languages (LanguageID)
	, MovieID			int			Foreign Key
		References Project.Movies (MovieID)
)

create table Project.Ratings (
	RatingsID			int		Primary Key Identity
	, NumberOfRatings	int
	, IMDBRating		float
	, MovieID			int		Foreign Key
			References Project.Movies (MovieID)
)

create table Project.Actors (
	ActorID		int			Primary Key Identity
	, Name		varchar(50)	not null
)

create table Project.Cast (
	CastID				int		Primary Key Identity
	, ActorID			int		Foreign Key
			references Project.Actors (ActorID)
	, MovieID			int		Foreign Key
			references Project.Movies (MovieID)
	, CharacterName		varchar(70)		not null
)

create table Project.Writers (
	WriterID		int			Primary Key Identity
	, WriterName	varchar(50)	not null
)

create table Project.MovieWriters (
	MovieWriterID	int			Primary Key Identity
	, WriterID		int			Foreign Key
			References Project.Writers (WriterID)
	, MovieID		int			Foreign Key
			References Project.Movies (MovieID)
)

create table Project.Directors (
	DirectorID	int		Primary Key Identity
	, DirectorName	varchar(50)
)
create table Project.MovieDirectors (
	MovieDirectorsID		int		Primary Key
	, DirectorID			int		Foreign Key
			References Project.Directors (DirectorID)
	, MovieID		int			Foreign Key
		References Project.Movies (MovieID)
)

create table Project.Genres (
	GenreID			int			Primary Key Identity
	, GenreName		varchar(25)	not null
)

create table Project.MovieGenre (
	MovieGenreID	int			Primary Key Identity
	, GenreID		int			Foreign Key
			References Project.Genres (GenreID)
	, MovieID		int			Foreign Key
			References Project.Movies (MovieID)
)
