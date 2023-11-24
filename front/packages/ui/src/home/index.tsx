/*
 * Kyoo - A portable and vast media library solution.
 * Copyright (c) Kyoo.
 *
 * See AUTHORS.md and LICENSE file in the project root for full license information.
 *
 * Kyoo is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * Kyoo is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Kyoo. If not, see <https://www.gnu.org/licenses/>.
 */

import { Genre, ItemKind, QueryPage } from "@kyoo/models";
import { Fetch } from "../fetch";
import { Header } from "./header";
import { DefaultLayout } from "../layout";
import { ScrollView } from "react-native";
import { GenreGrid } from "./genre";
import { Recommanded } from "./recommanded";
import { VerticalRecommanded } from "./vertical";
import { NewsList } from "./news";
import { useEffect, useState } from "react";

export const HomePage: QueryPage<{}, Genre> = ({ randomItems }) => {
	const [isClient, setClient] = useState(false);
	useEffect(() => setClient(true), []);

	return (
		<ScrollView>
			<Fetch query={Header.query()}>
				{(x) => (
					<Header
						isLoading={x.isLoading as any}
						name={x.name}
						tagline={"tagline" in x ? x.tagline : null}
						overview={x.overview}
						thumbnail={x.thumbnail}
						link={x.kind !== ItemKind.Collection && !x.isLoading ? x.playHref : undefined}
						infoLink={x.href}
					/>
				)}
			</Fetch>
			<NewsList />
			{randomItems
				.filter((_, i) => i < 2)
				.map((x) => (
					<GenreGrid key={x} genre={x} />
				))}
			<Recommanded />
			{randomItems
				.filter((_, i) => i >= 2 && i < 6)
				.map((x) => (
					<GenreGrid key={x} genre={x} />
				))}
			<VerticalRecommanded />
			{isClient && randomItems.filter((_, i) => i >= 6).map((x) => <GenreGrid key={x} genre={x} />)}
		</ScrollView>
	);
};

HomePage.randomItems = [...Object.values(Genre)];

HomePage.getLayout = { Layout: DefaultLayout, props: { transparent: true } };

HomePage.getFetchUrls = (_, randomItems) => [
	Header.query(),
	NewsList.query(),
	...randomItems.filter((_, i) => i < 6).map((x) => GenreGrid.query(x)),
	Recommanded.query(),
	VerticalRecommanded.query(),
];