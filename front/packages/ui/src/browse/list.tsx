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

import type { KyooImage, WatchStatusV } from "@kyoo/models";
import {
	GradientImageBackground,
	Heading,
	Link,
	P,
	Poster,
	PosterBackground,
	Skeleton,
	imageBorderRadius,
	important,
	ts,
} from "@kyoo/primitives";
import { useState } from "react";
import { Platform, View } from "react-native";
import { percent, px, rem, useYoshiki } from "yoshiki/native";
import { ItemContext } from "../components/context-menus";
import type { Layout } from "../fetch";
import { ItemWatchStatus } from "./grid";

export const ItemList = ({
	href,
	slug,
	type,
	name,
	subtitle,
	thumbnail,
	poster,
	watchStatus,
	unseenEpisodesCount,
	...props
}: {
	href: string;
	slug: string;
	type: "movie" | "show" | "collection";
	name: string;
	subtitle: string | null;
	poster: KyooImage | null;
	thumbnail: KyooImage | null;
	watchStatus: WatchStatusV | null;
	unseenEpisodesCount: number | null;
}) => {
	const { css } = useYoshiki("line");
	const [moreOpened, setMoreOpened] = useState(false);

	return (
		<GradientImageBackground
			src={thumbnail}
			alt={name}
			quality="medium"
			as={Link}
			href={moreOpened ? undefined : href}
			onLongPress={() => setMoreOpened(true)}
			{...css(
				{
					alignItems: "center",
					justifyContent: "space-evenly",
					flexDirection: "row",
					height: ItemList.layout.size,
					borderRadius: px(imageBorderRadius),
					overflow: "hidden",
					marginX: ItemList.layout.gap,
					child: {
						more: {
							opacity: 0,
						},
					},
					fover: {
						title: {
							textDecorationLine: "underline",
						},
						more: {
							opacity: 100,
						},
					},
				},
				props,
			)}
		>
			<View
				{...css({
					width: { xs: "50%", lg: "30%" },
				})}
			>
				<View
					{...css({
						flexDirection: "row",
						justifyContent: "center",
					})}
				>
					<Heading
						{...css([
							"title",
							{
								textAlign: "center",
								fontSize: rem(2),
								letterSpacing: rem(0.002),
								fontWeight: "900",
								textTransform: "uppercase",
							},
						])}
					>
						{name}
					</Heading>
					{type !== "collection" && (
						<ItemContext
							type={type}
							slug={slug}
							status={watchStatus}
							isOpen={moreOpened}
							setOpen={(v) => setMoreOpened(v)}
							{...css([
								{
									// I dont know why marginLeft gets overwritten by the margin: px(2) so we important
									marginLeft: important(ts(2)),
									bg: (theme) => theme.darkOverlay,
								},
								"more",
								Platform.OS === "web" && moreOpened && { opacity: important(100) },
							])}
						/>
					)}
				</View>
				{subtitle && (
					<P
						{...css({
							textAlign: "center",
							marginRight: ts(4),
						})}
					>
						{subtitle}
					</P>
				)}
			</View>
			<PosterBackground src={poster} alt="" quality="low" layout={{ height: percent(80) }}>
				<ItemWatchStatus watchStatus={watchStatus} unseenEpisodesCount={unseenEpisodesCount} />
			</PosterBackground>
		</GradientImageBackground>
	);
};

ItemList.Loader = (props: object) => {
	const { css } = useYoshiki();

	return (
		<View
			{...css(
				{
					alignItems: "center",
					justifyContent: "space-evenly",
					flexDirection: "row",
					height: ItemList.layout.size,
					borderRadius: px(imageBorderRadius),
					overflow: "hidden",
					bg: (theme) => theme.dark.background,
					marginX: ItemList.layout.gap,
				},
				props,
			)}
		>
			<View
				{...css({
					width: { xs: "50%", lg: "30%" },
					flexDirection: "column",
					justifyContent: "center",
				})}
			>
				<Skeleton {...css({ height: rem(2), alignSelf: "center" })} />
				<Skeleton {...css({ width: rem(5), alignSelf: "center" })} />
			</View>
			<Poster.Loader layout={{ height: percent(80) }} />
		</View>
	);
};

ItemList.layout = { numColumns: 1, size: 300, layout: "vertical", gap: ts(2) } satisfies Layout;
