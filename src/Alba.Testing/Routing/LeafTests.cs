﻿using System;
using System.Threading.Tasks;
using Alba.Routing;
using Shouldly;
using Xunit;

namespace Alba.Testing.Routing
{
    public class LeafTests
    {
        [Fact]
        public void blank_segment()
        {
            Leaf.ToParameter("foo", 0).ShouldBeOfType<Alba.Routing.Segment>().Path.ShouldBe("foo");
        }

        [Fact]
        public void spread()
        {
            Leaf.ToParameter("...", 4).ShouldBeOfType<Spread>()
                .Position.ShouldBe(4);
        }

        [Fact]
        public void argument_starting_with_colon()
        {
            var arg = Leaf.ToParameter(":foo", 2).ShouldBeOfType<RouteArgument>();
            arg.Position.ShouldBe(2);
            arg.Key.ShouldBe("foo");
        }

        [Fact]
        public void argument_in_brackets()
        {
            var arg = Leaf.ToParameter("{bar}", 3).ShouldBeOfType<RouteArgument>();
            arg.Position.ShouldBe(3);
            arg.Key.ShouldBe("bar");
        }

        [Fact]
        public void spread_has_to_be_last()
        {
            Action action = () =>
            {
                new Leaf("a/.../b", env => Task.CompletedTask);
            };
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void cannot_have_multiple_spreads_either()
        {
            Action action = () =>
            {
                new Leaf("a/.../b/...", env => Task.CompletedTask);
            };
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}