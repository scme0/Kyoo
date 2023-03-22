import os
from dataclasses import asdict, dataclass, field
from datetime import date
from typing import Optional

from .genre import Genre
from .status import Status
from .metadataid import MetadataID


@dataclass
class MovieTranslation:
	name: str
	tagline: Optional[str] = None
	keywords: list[str] = field(default_factory=list)
	overview: Optional[str] = None

	posters: list[str] = field(default_factory=list)
	logos: list[str] = field(default_factory=list)
	trailers: list[str] = field(default_factory=list)
	thumbnails: list[str] = field(default_factory=list)


@dataclass
class Movie:
	original_language: Optional[str] = None
	aliases: list[str] = field(default_factory=list)
	release_date: Optional[date | int] = None
	status: Status = Status.UNKNOWN
	path: Optional[str] = None
	studios: list[str] = field(default_factory=list)
	genres: list[Genre] = field(default_factory=list)
	# TODO: handle staff
	# staff: list[Staff]
	external_id: dict[str, MetadataID] = field(default_factory=dict)

	translations: dict[str, MovieTranslation] = field(default_factory=dict)

	def to_kyoo(self):
		# For now, the API of kyoo only support one language so we remove the others.
		default_language = os.environ["LIBRARY_LANGUAGES"].split(",")[0]
		return {
			**asdict(self),
			**asdict(self.translations[default_language]),
			"poster": next(iter(self.translations[default_language].posters), None),
			"thumbnail": next(iter(self.translations[default_language].thumbnails), None),
			"logo": next(iter(self.translations[default_language].logos), None),
			"trailer": next(iter(self.translations[default_language].trailers), None),
			"studio": next(iter(self.studios), None),
			"startAir": self.release_date,
			"title": self.translations[default_language].name,
			"isMovie": True,
		}